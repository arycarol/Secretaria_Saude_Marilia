// =====================================================
// PEGAR TOKEN DO COOKIE
// =====================================================
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

if (!token) {
    alert("Você precisa estar logado para acessar esta página.");
    window.location.href = "../login/login.html";
}



// =====================================================
// ENVIAR SOLICITAÇÃO (POST)
// =====================================================
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

    // Ajustar USER ID (depois posso extrair do token)

    // Body REAL da API
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

        carregarSolicitacoes();
        document.querySelector("form").reset();

    } catch (error) {
        console.error("Erro:", error);
        alert("Falha ao conectar com a API.");
    }
});



// =====================================================
// CARREGAR TODAS AS SOLICITAÇÕES (GET)
// =====================================================
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



// =====================================================
// PREENCHER TABELA
// =====================================================
function preencherTabela(lista) {
    const tabela = document.querySelector(".tabela-historico-figma tbody");
    tabela.innerHTML = "";

    lista.forEach(item => {
        tabela.innerHTML += `
            <tr>
                <td>${String(item.id).padStart(6, "0")}</td>
                <td>${item.originLocation}</td>
                <td>${item.destinationLocation}</td>
                <td>${item.date}</td>
                <td>${item.hour.substring(0,5)}</td>
                <td>${item.transportKind}</td>
                <td>${item.transportStatus}</td>
                <td>...</td>
            </tr>
        `;
    });
}



// =====================================================
// LOGOUT
// =====================================================
document.querySelector(".logout-icon-figma").addEventListener("click", () => {
    document.cookie = "jwt=; path=/; max-age=0";
    window.location.href = "../login/login.html";
});



// =====================================================
// INICIALIZAÇÃO
// =====================================================
carregarSolicitacoes();
