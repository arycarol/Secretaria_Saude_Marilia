// ======================================================================
// FUNÇÃO PARA PEGAR COOKIES
// ======================================================================
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

// ======================================================================
// PROTEÇÃO DO PAINEL ADMINISTRATIVO
// ======================================================================
if (!token) {
    alert("Você precisa estar logado.");
    window.location.href = "../login/login.html";
}

if (userCategory !== "Administrador" && userCategory !== "Admin") {
    alert("Acesso negado! Apenas administradores.");
    window.location.href = "../login/login.html";
}



// ======================================================================
// COMPONENTE VUE
// ======================================================================
const GerenciamentoUsuarios = {
    data() {
        return {
            isModalOpen: false,

            // Lista REAL de usuários vindos da API
            users: []
        };
    },

    // Chamado quando o Vue carrega
    mounted() {
        this.carregarUsuarios();
    },

    methods: {

        // ==================================================================
        // GET USERS - BUSCA TODOS OS USUÁRIOS NA API
        // ==================================================================
        async carregarUsuarios() {
            try {
                const response = await fetch("http://localhost:5260/api/v1/User", {
                    method: "GET",
                    headers: {
                        "Authorization": `Bearer ${token}`
                    }
                });

                if (!response.ok) {
                    alert("Erro ao carregar usuários!");
                    return;
                }

                const lista = await response.json();

                // Converter para o formato usado na tabela Vue
                this.users = lista.map(u => ({
                    ativo: true,
                    codigo: String(u.id).padStart(4, "0"),
                    nome: u.name,
                    email: u.email,
                    cpf: u.cpf,
                    telefone: u.telephone,
                    dataNascimento: this.formatarData(u.birthDate)
                }));

            } catch (error) {
                console.error("Erro ao buscar usuários:", error);
                alert("Erro de conexão ao buscar usuários.");
            }
        },



        // ==================================================================
        // POST - CADASTRAR NOVO USUÁRIO
        // ==================================================================
        async cadastrarUsuario() {
            const campos = document.querySelectorAll(".modal-content input");

            const nome = campos[0].value;
            const email = campos[1].value;
            const cpf = campos[2].value.replace(/\D/g, "");
            const telefone = campos[3].value;
            const dataNasc = campos[4].value;
            const senha = campos[5].value;
            const confirmarSenha = campos[6].value;
            const categoria = campos[7].value;

            if (!nome || !email || !cpf || !telefone || !dataNasc || !senha || !categoria) {
                alert("Preencha todos os campos!");
                return;
            }

            if (senha !== confirmarSenha) {
                alert("As senhas não coincidem!");
                return;
            }

            const novoUsuario = {
                name: nome,
                cpf: cpf,
                email: email,
                telephone: telefone,
                birthDate: dataNasc,
                password: senha,
                userCategory: categoria,
                vehicleId: null
            };

            try {
                const response = await fetch("http://localhost:5260/api/v1/User", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                        "Authorization": `Bearer ${token}`
                    },
                    body: JSON.stringify(novoUsuario)
                });

                if (!response.ok) {
                    const err = await response.text();
                    alert("Erro ao cadastrar usuário:\n" + err);
                    return;
                }

                this.closeModal();
                this.carregarUsuarios();

            } catch (error) {
                console.error("Erro no cadastro:", error);
                alert("Erro ao conectar com a API.");
            }
        },



        // ==================================================================
        // UTILS
        // ==================================================================
        toggleActive(codigo) {
            const user = this.users.find(u => u.codigo === codigo);
            if (user) user.ativo = !user.ativo;
        },

        formatarContadorRegistros(count) {
            return `${String(count).padStart(2, "0")} Registros`;
        },

        formatarData(dataStr) {
            if (!dataStr) return "";
            const data = new Date(dataStr);
            return data.toLocaleDateString("pt-BR");
        },



        // ==================================================================
        // MODAL CONTROLS
        // ==================================================================
        openModal() {
            this.isModalOpen = true;
        },

        closeModal() {
            this.isModalOpen = false;
        }
    }
};



// ======================================================================
// MONTAR A APLICAÇÃO VUE
// ======================================================================
window.addEventListener("DOMContentLoaded", () => {
    Vue.createApp(GerenciamentoUsuarios).mount("#app");
});
