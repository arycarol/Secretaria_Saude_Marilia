const { createApp } = Vue;

// Mapeamento para garantir que a classe CSS de status esteja correta
const statusMap = {
    'Em funcionamento': 'em-func',
    'Em uso': 'em-uso',
    'Manutenção': 'manut',
    'Quebrado': 'quebrado'
};

// Mapeamento inverso (usado para pré-preencher o select na edição)
const statusMapReverse = {
    'em-func': 'Em funcionamento',
    'em-uso': 'Em uso',
    'manut': 'Manutenção',
    'quebrado': 'Quebrado'
};

// Objeto para resetar o formulário
const defaultNewVehicle = {
    placa: '',
    renavam: '',
    modelo: '',
    ano: '',
    cor: '',
    capacidade: '',
    status: '', // Ex: 'Em funcionamento' (texto)
    categoria: '',
    combustivel: [], // Array para seleção múltipla
};

createApp({
    data() {
        return {
            isSidebarActive: true, 
            searchQuery: '',
            activeTab: 'emFunc',

            // MODAL CRIAÇÃO/EDIÇÃO
            isModalOpen: false, 
            modalMode: 'create', // 'create' ou 'edit'
            newVehicle: { ...defaultNewVehicle }, 
            originalPlaca: null, // Armazena a placa original para encontrar o item na edição

            // MODAL EXCLUSÃO
            isDeleteModalOpen: false,
            vehicleToDelete: {}, // Armazena o objeto do veículo a ser excluído

            // Listas de opções
            statusOptions: [
                { value: 'Em funcionamento', text: 'Em funcionamento' },
                { value: 'Em uso', text: 'Em Uso' },
                { value: 'Manutenção', text: 'Manutenção' },
                { value: 'Quebrado', text: 'Quebrado' }
            ],
            categoriaOptions: ['Ambulância', 'Carro', 'Ônibus', 'Van'], 
            combustivelOptions: ['Diesel', 'Etanol', 'Etanol/Gasolina', 'Gasolina'], 

            // Dados iniciais
            vehicles: [
                { placa: 'ABC-1234', renavam: '01234567890', ano: '2019/2020', modelo: 'Volkswagen Gol 1.6', cor: 'Branco', combustivel: ['Diesel'], capacidade: '16 Lugares', status: 'em-func' },
                { placa: 'ABC-1024', renavam: '34567901123', ano: '2021/2024', modelo: 'Mercedes-Benz Sprinter', cor: 'Branco', combustivel: ['Diesel'], capacidade: '15 Lugares', status: 'em-uso' },
                { placa: 'PFR-1023', renavam: '34567890123', ano: '2021/2022', modelo: 'Mercedes-Benz', cor: 'Branco', combustivel: ['Diesel'], capacidade: '15 Lugares', status: 'quebrado' },
                { placa: 'PFR-4789', renavam: '45678901234', ano: '2017/2018', modelo: 'Volkswagen Comil', cor: 'Branco', combustivel: ['Diesel', 'Etanol'], capacidade: '16 Lugares', status: 'manut' },
            ]
        }
    },
    computed: {
        filteredVehicles() {
            return this.vehicles.filter(v => {
                let matchesTab = false;
                if (this.activeTab === 'emFunc') {
                    matchesTab = v.status === 'em-func' || v.status === 'em-uso';
                } else if (this.activeTab === 'manut') {
                    matchesTab = v.status === 'quebrado' || v.status === 'manut';
                }

                const searchLower = this.searchQuery.toLowerCase();
                const matchesSearch = v.placa.toLowerCase().includes(searchLower) ||
                    v.modelo.toLowerCase().includes(searchLower) ||
                    v.renavam.toLowerCase().includes(searchLower);

                return matchesTab && matchesSearch;
            });
        }
    },
    methods: {
        // Retorna a classe CSS de status
        statusClass(status) {
            return 'status-' + status;
        },
        
        // ===================================
        // FUNÇÕES DO MODAL DE CRIAÇÃO/EDIÇÃO
        // ===================================

        // Abre o modal de Cadastro ('create') ou Edição ('edit')
        openModal(mode, vehicleData = null) {
            this.modalMode = mode;
            this.newVehicle = { ...defaultNewVehicle }; // Reseta
            this.originalPlaca = null;

            if (mode === 'edit' && vehicleData) {
                // Ao editar, pré-preenche o formulário com os dados do veículo
                this.newVehicle = {
                    ...vehicleData,
                    // Converte a chave CSS de status (Ex: 'em-func') para o texto do Select (Ex: 'Em funcionamento')
                    status: statusMapReverse[vehicleData.status] || '', 
                    // Garante que combustivel seja um array (o v-model espera isso)
                    combustivel: Array.isArray(vehicleData.combustivel) ? vehicleData.combustivel : [vehicleData.combustivel],
                    categoria: vehicleData.categoria || '',
                };
                this.originalPlaca = vehicleData.placa; // Armazena para referência na hora de salvar
            }
            this.isModalOpen = true;
        },

        closeModal() {
            this.isModalOpen = false;
        },

        // Função Única para salvar (Criação ou Edição)
        saveVehicle() {
            // Converte o status de texto para a chave CSS
            const statusKey = statusMap[this.newVehicle.status] || 'em-func';

            const vehicleToSave = {
                placa: this.newVehicle.placa,
                renavam: this.newVehicle.renavam,
                modelo: this.newVehicle.modelo,
                ano: this.newVehicle.ano,
                cor: this.newVehicle.cor,
                capacidade: this.newVehicle.capacidade,
                status: statusKey, 
                combustivel: this.newVehicle.combustivel, 
                categoria: this.newVehicle.categoria || '', 
            };
            
            if (this.modalMode === 'create') {
                // LÓGICA DE CRIAÇÃO
                this.vehicles.push(vehicleToSave);
                console.log('Novo veículo cadastrado:', vehicleToSave);

            } else if (this.modalMode === 'edit') {
                // LÓGICA DE EDIÇÃO
                const index = this.vehicles.findIndex(v => v.placa === this.originalPlaca);
                if (index !== -1) {
                    this.vehicles[index] = vehicleToSave;
                    console.log('Veículo editado:', vehicleToSave);
                }
            }

            this.closeModal();
            this.newVehicle = { ...defaultNewVehicle }; // Limpa
        },


        // ===================================
        // FUNÇÕES DO MODAL DE EXCLUSÃO
        // ===================================
        
        // Abre o modal de confirmação de exclusão
        openDeleteConfirmation(vehicle) {
            this.vehicleToDelete = vehicle;
            this.isDeleteModalOpen = true;
        },

        closeDeleteConfirmation() {
            this.isDeleteModalOpen = false;
            this.vehicleToDelete = {};
        },

        // Remove o veículo da lista após a confirmação
        deleteVehicle() {
            const placa = this.vehicleToDelete.placa;
            const index = this.vehicles.findIndex(v => v.placa === placa);

            if (index !== -1) {
                // Remove o veículo usando o índice
                this.vehicles.splice(index, 1);
                console.log(`Veículo de placa ${placa} excluído.`);
            }

            // Fecha o modal de exclusão
            this.closeDeleteConfirmation();
        }
    }
}).mount('#app');