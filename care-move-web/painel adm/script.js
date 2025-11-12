// Objeto de configuração do componente Vue
const GerenciamentoUsuarios = {
    data() {
        return {
            // ESSENCIAL: Controla a visibilidade do modal
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
        // Funções para controle do modal
        openModal() {
            this.isModalOpen = true;
            // Mensagem de log para confirmar que a função foi chamada
            console.log('Modal aberto: isModalOpen = true'); 
        },
        closeModal() {
            this.isModalOpen = false;
            // Mensagem de log para confirmar que a função foi chamada
            console.log('Modal fechado: isModalOpen = false'); 
        }
    }
};

// HARD MOUNTING: Monta a aplicação Vue na DOM (incluindo o modal fora do #app)
window.addEventListener('DOMContentLoaded', () => {
    // Monta o componente principal na div com id="app"
    Vue.createApp(GerenciamentoUsuarios).mount('#app');
    console.log('Aplicação Vue montada.'); 
});