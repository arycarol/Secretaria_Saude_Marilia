const { createApp } = Vue;

createApp({
    data() {
        return {
            isSidebarActive: false, // NOVO: Controla se a sidebar está aberta (começa fechada)
            searchQuery: '',
            activeTab: 'emFunc', // 'emFunc' ou 'manut'
            // Dados de exemplo
            vehicles: [
                { placa: 'ABC-1224', renavam: '01234567890', ano: '2019/2020', modelo: 'Volkswagen Gol 1.6', cor: 'Branco', combustivel: 'Diesel', capacidade: '16 Lugares', status: 'emFunc' },
                { placa: 'ABC-1024', renavam: '34567890123', ano: '2019/2023', modelo: 'Mercedes-Benz', cor: 'Branco', combustivel: 'Diesel', capacidade: '15 Lugares', status: 'emFunc' },
                { placa: 'PFR-1023', renavam: '34567901123', ano: '2021/2024', modelo: 'Mercedes-Benz Sprinter', cor: 'Branco', combustivel: 'Diesel', capacidade: '15 Lugares', status: 'emUso' }, 
                { placa: 'PFR-4783', renavam: '3data', ano: '2017/2018', modelo: 'Prata', cor: 'Branco', combustivel: 'Diesel', capacidade: '15 Lugares', status: 'quebrado' }, 
                { placa: 'PFR-4789', renavam: '45567901334', ano: '2017/2028', modelo: 'Volkswagen Comil', cor: 'Branco', combustivel: 'Diesel', capacidade: '16 Lugares', status: 'manut' }, 
            ]
        }
    },
    computed: {
        filteredVehicles() {
            // Filtra os veículos com base na aba ativa e na pesquisa
            return this.vehicles.filter(v => {
                
                // 1. Lógica de filtro por aba
                let matchesTab = false;
                if (this.activeTab === 'emFunc') {
                    // "Em funcionamento" inclui 'emFunc' e 'emUso'
                    matchesTab = v.status === 'emFunc' || v.status === 'emUso';
                } else if (this.activeTab === 'manut') {
                    // "Quebr/Manutenção" inclui 'quebrado' e 'manut'
                    matchesTab = v.status === 'quebrado' || v.status === 'manut';
                }
                
                // 2. Lógica de filtro por pesquisa
                const searchLower = this.searchQuery.toLowerCase();
                const matchesSearch = v.placa.toLowerCase().includes(searchLower) ||
                                      v.modelo.toLowerCase().includes(searchLower) ||
                                      v.renavam.toLowerCase().includes(searchLower);

                return matchesTab && matchesSearch;
            });
        }
    },
    methods: {
        // Retorna a classe CSS correta para o ponto de status
        statusClass(status) {
            if(status === 'emFunc') return 'status-em-func';
            if(status === 'quebrado') return 'status-quebrado';
            if(status === 'manut') return 'status-manut';
            if(status === 'emUso') return 'status-em-uso';
            return '';
        },
        addVehicle() {
            console.log('Funcionalidade de adicionar veículo iniciada.');
        }
    }
}).mount('#app');
