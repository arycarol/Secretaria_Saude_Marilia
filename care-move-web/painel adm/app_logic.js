// Objeto de configuração do componente Vue
const GerenciamentoUsuarios = {
    data() {
        return {
            // Estado para controlar a visibilidade do modal
            isModalOpen: false, 
            
            // REGISTROS SEPARADOS (Dados dinâmicos)
            users: [
                {
                    ativo: true,
                    codigo: '0001',
                    nome: 'Aryane Souza',
                    email: 'aryane.souza@gmail.com',
                    cpf: '488.121.316-05',
                    telefone: '(14)98736-0987',
                    dataNascimento: '28/10/2005'
                },
                {
                    ativo: true,
                    codigo: '0002',
                    nome: 'Sofia Deniz',
                    email: 'sofiadeniz05@gmail.com',
                    cpf: '480.101.987-04',
                    telefone: '(14)90987-5756',
                    dataNascimento: '23/10/2005'
                },
                {
                    ativo: true,
                    codigo: '0003',
                    nome: 'Jeniffer Scarpin',
                    email: 'jeniffer.scarpin@gmail.com',
                    cpf: '000.101.987-04',
                    telefone: '(14)98148-0988',
                    dataNascimento: '08/05/2005'
                },
                {
                    ativo: true,
                    codigo: '0004',
                    nome: 'Rafael Honório',
                    email: 'rafael.honorio@gmail.com',
                    cpf: '480.009.122-01',
                    telefone: '(14)98119-2877',
                    dataNascimento: '08/09/2005'
                }
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
            return `${String(count).padStart(2, '0')} Registros`;
        },
        openModal() {
            this.isModalOpen = true;
        },
        closeModal() {
            this.isModalOpen = false;
        }
    }
};

// Cria e monta a aplicação Vue no elemento com id="app"
Vue.createApp(GerenciamentoUsuarios).mount('#app');