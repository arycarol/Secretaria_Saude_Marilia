const { createApp } = Vue;

createApp({
    data() {
        return {
            // ALTERAÇÃO 1: Começa como true para a sidebar iniciar aberta (como na 3ª imagem)
            isSidebarActive: true, 
            searchQuery: '',
            activeTab: 'emFunc', // 'emFunc' ou 'manut'
            // ALTERAÇÃO 2: Dados de exemplo ajustados para corresponder mais ao protótipo
            vehicles: [
                { placa: 'ABC-1234', renavam: '01234567890', ano: '2019/2020', modelo: 'Volkswagen Gol 1.6', cor: 'Branco', combustivel: 'Diesel', capacidade: '16 Lugares', status: 'em-func' },
                // Veículo em 'quebrado' para aparecer na aba Quebr/Manutenção
                { placa: 'PFR-1023', renavam: '34567890123', ano: '2021/2022', modelo: 'Mercedes-Benz', cor: 'Branco', combustivel: 'Diesel', capacidade: '15 Lugares', status: 'quebrado' },
                // Veículo em 'manut' para aparecer na aba Quebr/Manutenção
                { placa: 'PFR-4789', renavam: '45678901234', ano: '2017/2018', modelo: 'Volkswagen Comil', cor: 'Branco', combustivel: 'Diesel', capacidade: '16 Lugares', status: 'manut' },
                // Veículo em 'em-uso' para aparecer na aba Em Funcionamento
                { placa: 'ABC-1024', renavam: '34567901123', ano: '2021/2024', modelo: 'Mercedes-Benz Sprinter', cor: 'Branco', combustivel: 'Diesel', capacidade: '15 Lugares', status: 'em-uso' },
            ]
        }
    },
    computed: {
        filteredVehicles() {
            return this.vehicles.filter(v => {

                // 1. Lógica de filtro por aba 
                let matchesTab = false;
                if (this.activeTab === 'emFunc') {
                    // "Em funcionamento" inclui 'em-func' e 'em-uso'
                    matchesTab = v.status === 'em-func' || v.status === 'em-uso';
                } else if (this.activeTab === 'manut') {
                    // "Quebr/Manutenção" inclui 'quebrado' e 'manut'
                    matchesTab = v.status === 'quebrado' || v.status === 'manut';
                }

                // 2. Lógica de filtro por pesquisa (mantida, pois funciona bem)
                const searchLower = this.searchQuery.toLowerCase();
                const matchesSearch = v.placa.toLowerCase().includes(searchLower) ||
                    v.modelo.toLowerCase().includes(searchLower) ||
                    v.renavam.toLowerCase().includes(searchLower);

                return matchesTab && matchesSearch;
            });
        }
    },
    methods: {
        // Retorna a classe CSS correta para o ponto de status (útil para a Legenda)
        statusClass(status) {
            if (status === 'em-func') return 'em-func';
            if (status === 'quebrado') return 'quebrado';
            if (status === 'manut') return 'manut';
            if (status === 'em-uso') return 'em-uso';
            return '';
        },
        addVehicle() {
            // A funcionalidade de adicionar veículo foi ativada, mas aqui é só o log
            console.log('Funcionalidade de adicionar veículo iniciada. (Pronto para implementação do modal)');
        }
    }
}).mount('#app');