const API_BASE_URL = "http://localhost:5260/api/v1/User";

function getCookie(name) {
    const cookies = document.cookie.split(";").map(c => c.trim());
    for (let cookie of cookies) {
        if (cookie.startsWith(name + "=")) {
            return cookie.substring(name.length + 1);
        }
    }
    return null;
}

const token = getCookie("jwt");
const userCategory = getCookie("userCategory");

if (!token) {
    alert("Você precisa estar logado.");
    window.location.href = "../login/login.html";
}

if (userCategory !== "Administrador" && userCategory !== "Admin") {
    alert("Acesso negado! Apenas administradores.");
    window.location.href = "../login/login.html";
}

const defaultNewUser = {
    ativo: true,
    codigo: null, 
    nome: "",
    email: "",
    cpf: "",
    telefone: "",
    dataNascimento: "",
    senha: "",
    confirmarSenha: "",
    categoria: "",
    id: null
};

const GerenciamentoUsuarios = {
    data() {
        return {
            isSidebarActive: false, 
            searchQuery: "",
            isModalOpen: false,
            modalMode: "create", 
            newUser: { ...defaultNewUser }, 
            isDeleteModalOpen: false,
            userToDelete: {}, 
            passwordVisible: false,
            confirmPasswordVisible: false,
            isSuccessModalOpen: false, 
            successMessage: {
                topLine: "",
                code: null,
                bottomLine: ""
            },
            users: []
        };
    },
    
    computed: {
        filteredUsers() {
            if (!this.searchQuery) return this.users; 
            
            const queryLowerCase = this.searchQuery.toLowerCase();
            const queryDigits = queryLowerCase.replace(/\D/g, "");

            return this.users.filter(u => {
                const stringMatch = (
                    u.nome.toLowerCase().includes(queryLowerCase) ||
                    u.email.toLowerCase().includes(queryLowerCase) ||
                    String(u.codigo).includes(queryLowerCase)
                );
                
                const numericMatch = (queryDigits.length >= 3) && (
                    u.cpf.replace(/\D/g, "").includes(queryDigits) ||
                    u.telefone.replace(/\D/g, "").includes(queryDigits) ||
                    u.dataNascimento.replace(/\D/g, "").includes(queryDigits)
                );

                return stringMatch || numericMatch;
            });
        }
    },

    mounted() {
        this.carregarUsuarios();
    },

    methods: {
        
        async carregarUsuarios() {
            try {
                const response = await fetch(API_BASE_URL, {
                    method: "GET",
                    headers: {
                        "Authorization": `Bearer ${token}`
                    }
                });

                if (!response.ok) {
                    throw new Error(`Erro ao carregar usuários: ${response.statusText}`);
                }

                const lista = await response.json();

                this.users = lista.map(u => ({
                    ativo: u.isActive || true,
                    codigo: String(u.id).padStart(4, "0"),
                    nome: u.name,
                    email: u.email,
                    cpf: u.cpf,
                    telefone: u.telephone,
                    dataNascimento: this.formatarData(u.birthDate, true),
                    id: u.id,
                    categoria: u.userCategory 
                }));

            } catch (error) {
                console.error("Falha ao buscar usuários:", error);
                alert("Erro de conexão ao buscar usuários. Verifique o console.");
            }
        },
        
        async saveUser() {
            if (this.modalMode === "create" && this.newUser.senha !== this.newUser.confirmarSenha) {
                alert("As senhas não coincidem!");
                return;
            }

            this.closeModal(); 

            const payload = {
                name: this.newUser.nome,
                cpf: this.newUser.cpf,
                email: this.newUser.email,
                telephone: this.newUser.telefone,
                birthDate: this.newUser.dataNascimento,
                password: this.newUser.senha,
                userCategory: this.newUser.categoria,
                vehicleId: null,
                isActive: this.newUser.ativo
            };
            
            let method, url;

            if (this.modalMode === "create") {
                method = 'POST';
                url = API_BASE_URL;
            } else {
                method = 'PUT';
                url = `${API_BASE_URL}/${this.newUser.id}`; 
                delete payload.password; 
            }

            try {
                const response = await fetch(url, {
                    method: method,
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${token}`
                    },
                    body: JSON.stringify(payload)
                });

                if (!response.ok) {
                    const errorData = await response.text();
                    throw new Error(`Erro na operação: ${errorData}`);
                }
                
                await this.carregarUsuarios(); 

                const code = this.newUser.codigo || 'Novo';
                this.openSuccessModal({ 
                    topLine: "Registro", 
                    code: code, 
                    bottomLine: this.modalMode === 'create' ? "cadastrado com sucesso!" : "alterado com sucesso!"
                });

            } catch (error) {
                console.error(`Falha ao ${this.modalMode === 'create' ? 'cadastrar' : 'salvar'} usuário:`, error);
                alert(`Erro ao tentar ${this.modalMode === 'create' ? 'cadastrar' : 'salvar'} usuário: ${error.message}`);
            }
        },

        async deleteUser() {
            const deletedUserId = this.userToDelete.id;
            const deletedUserCode = this.userToDelete.codigo;
            this.closeDeleteConfirmation();

            try {
                const url = `${API_BASE_URL}/${deletedUserId}`; 
                
                const response = await fetch(url, {
                    method: 'DELETE',
                    headers: {
                        'Authorization': `Bearer ${token}`
                    }
                });

                if (!response.ok) {
                    const errorData = await response.text();
                    throw new Error(`Erro ao excluir: ${errorData}`);
                }

                this.users = this.users.filter(u => u.id !== deletedUserId);
                
                this.openSuccessModal({ 
                    topLine: "Registro", 
                    code: deletedUserCode, 
                    bottomLine: "excluído com sucesso!" 
                });

            } catch (error) {
                console.error("Falha ao excluir usuário:", error);
                alert(`Erro ao excluir usuário: ${error.message}`);
            }
        },
        
        async toggleActive(codigo) {
            const user = this.users.find(u => u.codigo === codigo);
            if (!user) return;
            
            const newStatus = !user.ativo;

            try {
                const url = `${API_BASE_URL}/${user.id}`; 
                
                const response = await fetch(url, {
                    method: 'PUT',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${token}`
                    },
                    body: JSON.stringify({ ...user, isActive: newStatus })
                });

                if (!response.ok) {
                    const errorData = await response.text();
                    throw new Error(`Erro ao alterar status: ${errorData}`);
                }

                user.ativo = newStatus;
                
            } catch (error) {
                console.error("Falha ao alterar status de ativo:", error);
                alert(`Erro ao alterar o status do usuário ${codigo}: ${error.message}`);
            }
        },
        
        openModal(mode, user = null) {
            this.modalMode = mode;
            this.passwordVisible = false;
            this.confirmPasswordVisible = false;

            if (mode === "create") {
                this.newUser = { ...defaultNewUser };
            } else {
                this.newUser = {
                    ...user,
                    senha: "", 
                    confirmarSenha: ""
                }; 
            }
            this.isModalOpen = true;
        },

        closeModal() {
            this.isModalOpen = false;
        },

        openDeleteConfirmation(user) {
            this.userToDelete = user;
            this.isDeleteModalOpen = true;
        },

        closeDeleteConfirmation() {
            this.isDeleteModalOpen = false;
            this.userToDelete = {};
        },

        openSuccessModal(messageData) {
            this.successMessage = messageData;
            this.isSuccessModalOpen = true;
        },
        
        closeSuccessModal() {
            this.isSuccessModalOpen = false;
            this.successMessage = { topLine: "", code: null, bottomLine: "" };
        },

        formatarData(dataStr, returnDigitsOnly = false) {
            if (!dataStr) return "";
            
            let data;
            if (dataStr.includes('/')) {
                const parts = dataStr.split('/');
                data = new Date(`${parts[1]}/${parts[0]}/${parts[2]}`);
            } else {
                data = new Date(dataStr);
            }

            if (isNaN(data)) {
                return returnDigitsOnly ? dataStr.replace(/\D/g, '').substring(0, 8) : dataStr;
            }
            
            const dia = String(data.getDate()).padStart(2, '0');
            const mes = String(data.getMonth() + 1).padStart(2, '0');
            const ano = data.getFullYear();
            
            if (returnDigitsOnly) {
                return `${dia}${mes}${ano}`; 
            } else {
                return `${dia}/${mes}/${ano}`; 
            }
        },

        formatarContadorRegistros() {
            return `${this.filteredUsers.length} Registros`; 
        },

        formatCpf(cpf) {
            cpf = String(cpf).replace(/\D/g, '').substring(0, 11);
            if (cpf.length <= 3) return cpf;
            if (cpf.length <= 6) return `${cpf.substring(0, 3)}.${cpf.substring(3)}`;
            if (cpf.length <= 9) return `${cpf.substring(0, 3)}.${cpf.substring(3, 6)}.${cpf.substring(6)}`;
            return `${cpf.substring(0, 3)}.${cpf.substring(3, 6)}.${cpf.substring(6, 9)}-${cpf.substring(9, 11)}`;
        },
        
        updateCpf(value) {
            this.newUser.cpf = value.replace(/\D/g, '').substring(0, 11); 
        },

        formatarTelefone(telefone) {
            if (!telefone) return "";
            let digits = String(telefone).replace(/\D/g, '').substring(0, 11); 
            
            if (digits.length <= 2) return `(${digits}`;
            if (digits.length <= 7) return `(${digits.substring(0, 2)}) ${digits.substring(2)}`;
            if (digits.length <= 11) return `(${digits.substring(0, 2)}) ${digits.substring(2, 7)}-${digits.substring(7)}`;
            
            return `(${digits.substring(0, 2)}) ${digits.substring(2, 7)}-${digits.substring(7, 11)}`;
        },

        updateTelefone(value) {
            this.newUser.telefone = value.replace(/\D/g, '').substring(0, 11); 
        },
        
        formatarDataNascimento(dataStr) {
            if (!dataStr) return "";
            let digits = String(dataStr).replace(/\D/g, '').substring(0, 8); 

            if (digits.length <= 2) return digits;
            if (digits.length <= 4) return `${digits.substring(0, 2)}/${digits.substring(2)}`;
            return `${digits.substring(0, 2)}/${digits.substring(2, 4)}/${digits.substring(4)}`;
        },

        updateDataNascimento(value) {
            this.newUser.dataNascimento = value.replace(/\D/g, '').substring(0, 8);
        },

        togglePasswordVisibility() {
            this.passwordVisible = !this.passwordVisible;
        },

        toggleConfirmPasswordVisibility() {
            this.confirmPasswordVisible = !this.confirmPasswordVisible;
        },
    }
};

window.addEventListener("DOMContentLoaded", () => {
    Vue.createApp(GerenciamentoUsuarios).mount("#app");
});