const { createApp } = Vue;

// Mapeamento para status visual (CSS)
const statusMap = {
    "Em funcionamento": "em-func",
    "Em uso": "em-uso",
    "Manutenção": "manut",
    "Quebrado": "quebrado"
};

const statusMapReverse = {
    "em-func": "Em funcionamento",
    "em-uso": "Em uso",
    "manut": "Manutenção",
    "quebrado": "Quebrado"
};

// Modelo base do formulário
const defaultNewVehicle = {
    id: null,
    placa: "",
    renavam: "",
    modelo: "",
    ano: "",
    cor: "",
    capacidade: "",
    categoria: "",
    combustivel: "",
    status: ""
};

createApp({
    data() {
        return {
            isSidebarActive: false,
            searchQuery: "",
            activeTab: "emFunc",

            isModalOpen: false,
            modalMode: "create",
            newVehicle: { ...defaultNewVehicle },

            isDeleteModalOpen: false,
            vehicleToDelete: {},

            statusOptions: [
                { value: "Em funcionamento", text: "Em funcionamento" },
                { value: "Em uso", text: "Em Uso" },
                { value: "Manutenção", text: "Manutenção" },
                { value: "Quebrado", text: "Quebrado" }
            ],
            categoriaOptions: ["Ambulância", "Carro", "Ônibus", "Van"],
            combustivelOptions: ["Diesel", "Etanol", "Etanol/Gasolina", "Gasolina"],

            vehicles: [] // <-- Recebe da API
        };
    },

    computed: {
        filteredVehicles() {
            return this.vehicles.filter(v => {
                let matchesTab = false;

                // FILTRO DA ABA
                if (this.activeTab === "emFunc") {
                    matchesTab = v.status === "em-func" || v.status === "em-uso";
                } else {
                    matchesTab = v.status === "quebrado" || v.status === "manut";
                }

                const s = this.searchQuery.toLowerCase();

                const matchesSearch =
                    v.placa.toLowerCase().includes(s) ||
                    v.modelo.toLowerCase().includes(s) ||
                    v.renavam.toLowerCase().includes(s);

                return matchesTab && matchesSearch;
            });
        }
    },

    methods: {
        // Lê o JWT salvo no cookie
        getToken() {
            return document.cookie
                .split("; ")
                .find(c => c.startsWith("jwt="))
                ?.split("=")[1];
        },

        statusClass(s) {
            return "status-" + s;
        },

        // ============================================================
        // 📌 CARREGAR VEÍCULOS DA API
        // ============================================================
        async loadVehicles() {
            try {
                const token = this.getToken();

                const res = await fetch("http://localhost:5260/api/v1/Vehicle", {
                    headers: { "Authorization": `Bearer ${token}` }
                });

                if (!res.ok) throw new Error("Erro API GET");

                const data = await res.json();

                // 🔁 Mapeamento correto dos campos API → Front
                this.vehicles = data.map(v => ({
                    id: v.id,
                    placa: v.licensePlate,
                    renavam: v.renavam,
                    modelo: v.vehicleModel,
                    ano: v.year,
                    cor: v.color,
                    combustivel: v.vehicleFuelType,
                    capacidade: v.capacity,
                    categoria: v.vehicleCategory,
                    status: statusMap[v.vehicleStatus] || "em-func"
                }));

            } catch (e) {
                console.error("Erro loadVehicles():", e);
                alert("Erro ao carregar veículos");
            }
        },

        // ============================================================
        // 📌 ABRIR MODAL (criar/editar)
        // ============================================================
        openModal(mode, vehicle = null) {
            this.modalMode = mode;

            if (mode === "create") {
                this.newVehicle = { ...defaultNewVehicle };
            } else {
                this.newVehicle = {
                    id: vehicle.id,
                    placa: vehicle.placa,
                    renavam: vehicle.renavam,
                    modelo: vehicle.modelo,
                    ano: vehicle.ano,
                    cor: vehicle.cor,
                    capacidade: vehicle.capacidade,
                    categoria: vehicle.categoria,
                    combustivel: vehicle.combustivel,
                    status: statusMapReverse[vehicle.status]
                };
            }

            this.isModalOpen = true;
        },

        closeModal() {
            this.isModalOpen = false;
        },

        // ============================================================
        // 📌 SALVAR VEÍCULO (POST / PUT)
        // ============================================================
        async saveVehicle() {
            try {
                const token = this.getToken();

                const payload = {
                    id: this.newVehicle.id,
                    name: this.newVehicle.modelo,
                    licensePlate: this.newVehicle.placa,
                    vehicleCategory: this.newVehicle.categoria,
                    vehicleModel: this.newVehicle.modelo,
                    color: this.newVehicle.cor,
                    vehicleFuelType: this.newVehicle.combustivel,
                    vehicleStatus: this.newVehicle.status,
                    renavam: this.newVehicle.renavam,
                    year: this.newVehicle.ano,
                    capacity: Number(this.newVehicle.capacidade)
                };

                const url = "http://localhost:5260/api/v1/Vehicle";
                const method = this.modalMode === "create" ? "POST" : "PUT";

                const res = await fetch(url, {
                    method,
                    headers: {
                        "Content-Type": "application/json",
                        "Authorization": `Bearer ${token}`
                    },
                    body: JSON.stringify(payload)
                });

                if (!res.ok) {
                    const t = await res.text();
                    console.log("Erro API:", t);
                    throw new Error("Falha ao salvar");
                }

                alert(this.modalMode === "create" ? "Veículo criado!" : "Atualizado!");

                this.closeModal();
                this.loadVehicles();

            } catch (e) {
                console.error("Erro saveVehicle():", e);
                alert("Erro ao salvar veículo");
            }
        },

        // ============================================================
        // 📌 MODAL DE EXCLUSÃO
        // ============================================================
        openDeleteConfirmation(vehicle) {
            this.vehicleToDelete = vehicle;
            this.isDeleteModalOpen = true;
        },

        closeDeleteConfirmation() {
            this.isDeleteModalOpen = false;
            this.vehicleToDelete = {};
        },

        // ============================================================
        // 📌 EXCLUIR VEÍCULO (DELETE)
        // ============================================================
        async deleteVehicle() {
            try {
                const token = this.getToken();

                const res = await fetch("http://localhost:5260/api/v1/Vehicle", {
                    method: "DELETE",
                    headers: {
                        "Content-Type": "application/json",
                        "Authorization": `Bearer ${token}`
                    },
                    body: JSON.stringify({ id: this.vehicleToDelete.id })
                });

                if (!res.ok) throw new Error("Erro ao excluir");

                alert("Veículo excluído!");

                this.closeDeleteConfirmation();
                this.loadVehicles();

            } catch (e) {
                console.error("Erro deleteVehicle():", e);
                alert("Erro ao excluir veículo");
            }
        }
    },

    mounted() {
        this.loadVehicles();
    }
}).mount("#app");
