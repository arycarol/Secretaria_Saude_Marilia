// Objeto de configuração do componente Vue
const GerenciamentoUsuarios = {
    data() {
        return {
            // ESSENCIAL: Controla a visibilidade do modal (mantido, mas agora o openModal não o usa)
            isModalOpen: false, 
            
            // REGISTROS DOS USUÁRIOS (Mock Data)
            users: [
                { ativo: true, codigo: '0001', nome: 'Aryane Souza', email: 'aryane.souza@gmail.com', cpf: '488.121.316-05', telefone: '(14)98736-0987', dataNascimento: '28/10/2005' },
                { ativo: true, codigo: '0002', nome: 'Sofia Deniz', email: 'sofiadeniz05@gmail.com', cpf: '480.101.987-04', telefone: '(14)90987-5756', dataNascimento: '23/10/2005' },
                { ativo: true, codigo: '0003', nome: 'Jeniffer Scarpin', email: 'jeniffer.scarpin@gmail.com', cpf: '000.101.987-04', telefone: '(14)98148-0988', dataNascimento: '08/05/2005' },
                { ativo: true, codigo: '0004', nome: 'Rafael Honório', email: 'rafael.honorio@gmail.com', cpf: '480.009.122-01', telefone: '(14)98119-2877', dataNascimento: '08/09/2005' }
            ]
        };
    },
    methods: {
        toggleActive(codigo) {
            const user = this.users.find(u => u.codigo === codigo);
            if (user) {
                user.ativo = !user.ativo;
            }
        },
        formatarContadorRegistros(count) {
            // Formata o contador para sempre ter dois dígitos (ex: 04 Registros)
            return `${String(count).padStart(2, '0')} Registros`;
        },
        // FUNÇÃO MODIFICADA PARA REDIRECIONAR EM VEZ DE ABRIR O MODAL
        openModal() {
            // Redireciona o navegador para o arquivo painel_admin.html
            console.log('Redirecionando para painel_admin.html...');
            window.location.href = 'painel_admin.html'; 
        },
        closeModal() {
            this.isModalOpen = false;
            // Mensagem de log para confirmar que a função foi chamada
            console.log('Modal fechado: isModalOpen = false'); 
        }
    }
};

// Monta o componente principal na div com id="app"
Vue.createApp(GerenciamentoUsuarios).mount('#app');
console.log('Aplicação Vue montada.');