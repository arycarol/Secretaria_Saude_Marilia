function getToken() {
    const cookies = document.cookie.split(";");
    for (let c of cookies) {
        const [name, value] = c.trim().split("=");
        if (name === "jwt") return value;
    }
    return null;
}

function getUserId() {
    const cookies = document.cookie.split(";");
    for (let c of cookies) {
        const [name, value] = c.trim().split("=");
        if (name === "userId") return value;
    }
    return null;
}

const token = getToken();
const userId = getUserId();

if (!token || !userId) {
    alert("Você precisa estar logado para acessar esta página.");
    window.location.href = "../login/login.html"; 
}

function openSuccessModal() {
    document.getElementById('success-modal-overlay').style.display = 'flex';
}

function closeSuccessModal() {
    document.getElementById('success-modal-overlay').style.display = 'none';
}

document.addEventListener("DOMContentLoaded", () => {
    const sidebar = document.getElementById("sidebar-figma");
    const toggleButton = document.getElementById("menu-toggle-button");
    const pageContentWrapper = document.getElementById("page-content-wrapper"); 
    const toggleIcon = toggleButton.querySelector('i');
    
    function setSidebarState(isActive) {
        sidebar.classList.toggle('active', isActive);
        toggleButton.classList.toggle('active', isActive);
        pageContentWrapper.classList.toggle('shifted', isActive); 
        
        if (isActive) {
            toggleIcon.classList.remove('fa-bars');
            toggleIcon.classList.add('fa-chevron-left');
        } else {
            toggleIcon.classList.remove('fa-chevron-left');
            toggleIcon.classList.add('fa-bars');
        }
    }
    
    toggleButton.addEventListener("mouseenter", () => {
        setSidebarState(true);
    });
    
    const handleMouseLeave = () => {
        setSidebarState(false);
    };

    toggleButton.addEventListener("mouseleave", handleMouseLeave);
    sidebar.addEventListener("mouseleave", handleMouseLeave);
    sidebar.addEventListener("mouseenter", () => setSidebarState(true));
});

document.querySelector(".btn-enviar-figma").addEventListener("click", async (event) => {
    event.preventDefault();

    const partida = document.getElementById("partida").value;
    const chegada = document.getElementById("chegada").value;
    const atendimento = document.getElementById("atendimento").value;
    const data = document.getElementById("data").value;
    const hora = document.getElementById("hora").value;

    if (!partida || !chegada || !atendimento || !data || !hora) {
        alert("Preencha todos os campos!");
        return;
    }

    const requestBody = {
        userId: userId,
        date: data,
        hour: hora,
        transportKind: atendimento,
        transportStatus: "Aguardando",
        originLocation: partida,
        destinationLocation: chegada
    };

    try {
        const response = await fetch("http://localhost:5260/api/v1/TransportRequest", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify(requestBody)
        });

        if (!response.ok) {
            const err = await response.text();
            alert("Erro ao enviar solicitação:\n" + err);
            return;
        }

        document.querySelector("form").reset();
        openSuccessModal();
        carregarSolicitacoes();

    } catch (error) {
        console.error("Erro:", error);
        alert("Falha ao conectar com a API.");
    }
});

async function carregarSolicitacoes() {
    try {
        const response = await fetch(`http://localhost:5260/api/v1/TransportRequest/GetListByUserId/${userId}`, {
            method: "GET",
            headers: {
                "Authorization": `Bearer ${token}`
            }
        });

        if (!response.ok) {
            alert("Erro ao carregar solicitações.");
            return;
        }

        const lista = await response.json();
        preencherTabela(lista);

    } catch (error) {
        console.log("Erro ao buscar solicitações:", error);
    }
}

function preencherTabela(lista) {
    const tabela = document.querySelector(".tabela-historico-figma tbody");
    tabela.innerHTML = "";

    const listaOrdenada = lista.sort((a, b) => b.id - a.id);

    listaOrdenada.forEach(item => {
        const statusClass = item.transportStatus.toLowerCase().replace(/[áã]/g, 'a').replace(/ú/g, 'u').replace(/ /g, '');
        
        tabela.innerHTML += `
            <tr>
                <td>${String(item.id).padStart(6, "0")}</td>
                <td>${item.originLocation}</td>
                <td>${item.destinationLocation}</td>
                <td>${item.date}</td>
                <td>${item.hour.substring(0,5)}</td>
                <td>${item.transportKind}</td>
                <td title="${item.transportStatus}"><span class="bolinha-figma ${statusClass}"></span></td>
            </tr>
        `;
    });
}

document.querySelector(".logout-icon-figma").addEventListener("click", () => {
    document.cookie = "jwt=; path=/; max-age=0";
    document.cookie = "userId=; path=/; max-age=0";
    window.location.href = "../login/login.html";
});

carregarSolicitacoes();