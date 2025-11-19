// ================================
//  PEGAR TOKEN DO COOKIE
// ================================
function getCookie(name) {
    const cookies = document.cookie.split(";").map(c => c.trim());
    for (const cookie of cookies) {
        if (cookie.startsWith(name + "=")) {
            return cookie.substring(name.length + 1);
        }
    }
    return null;
}

const token = getCookie("jwt");
if (!token) {
    alert("Sessão expirada. Faça login novamente.");
    window.location.href = "../login/login.html";
}

const API_BASE = "http://localhost:5260/api/v1";

// ================================
//  ELEMENTOS
// ================================
const solicitacoesContainer = document.querySelector(".lista-solicitacoes");
const agendaContainer = document.querySelector(".lista-agenda");

// ================================
//  CARREGAR ASSIGNMENTS DO DIA
// ================================
async function carregarAssignments() {
    try {
        const response = await fetch(`${API_BASE}/TransportAssignment`, {
            headers: {
                "Authorization": `Bearer ${token}`
            }
        });

        if (!response.ok) throw new Error("Erro ao buscar solicitações");

        const assignments = await response.json();
        solicitacoesContainer.innerHTML = "";

        if (assignments.length === 0) {
            solicitacoesContainer.innerHTML =
                `<p class="mensagem-vazia">Nenhuma solicitação nova hoje.</p>`;
            return;
        }

        for (const item of assignments) {
            const paciente = await carregarPaciente(item.pacientUserId);
            const request = await carregarRequest(item.transportRequestId);

            adicionarSolicitacao(
                item.id,
                request.hour,
                paciente.name,
                paciente.cpf,
                request.tipoAtendimento,
                request.transportKind,
                request.originLocation
            );
        }
    } catch (err) {
        console.error(err);
        solicitacoesContainer.innerHTML =
            `<p class="mensagem-vazia">Erro ao carregar solicitações.</p>`;
    }
}

// ================================
//  BUSCAR PACIENTE
// ================================
async function carregarPaciente(id) {
    const res = await fetch(`${API_BASE}/User/${id}`, {
        headers: { "Authorization": `Bearer ${token}` }
    });
    return await res.json();
}

// ================================
//  BUSCAR REQUEST
// ================================
async function carregarRequest(id) {
    const res = await fetch(`${API_BASE}/TransportRequest/${id}`, {
        headers: { "Authorization": `Bearer ${token}` }
    });
    return await res.json();
}

// ================================
//  ADICIONAR CARD NA TELA
// ================================
function adicionarSolicitacao(id, hora, nome, cpf, tipoAtendimento, local) {
    const div = document.createElement("div");
    div.classList.add("box-info");

    div.innerHTML = `
      <p><strong>${hora} - ${nome} - CPF: ${cpf}</strong></p>
      <p class="descricao">${tipoAtendimento}</p>
      <p><strong>LOCAL:</strong> ${local}</p>

      <div class="botoes">
        <button class="btn-recusar">RECUSAR</button>
        <button class="btn-aceitar">ACEITAR</button>
      </div>
    `;

    div.querySelector(".btn-aceitar").addEventListener("click", () => aceitarSolicitacao(id, div));
    solicitacoesContainer.appendChild(div);
}

// ================================
//  ACEITAR SOLICITAÇÃO
// ================================
async function aceitarSolicitacao(id, elemento) {
    try {
        await fetch(`${API_BASE}/TransportAssignment/Accept/${id}`, {
            method: "PUT",
            headers: { "Authorization": `Bearer ${token}` }
        });

        elemento.remove();
        mostrarModal();

    } catch (err) {
        alert("Erro ao aceitar solicitação.");
        console.error(err);
    }
}

// ================================
//  MODAL DE SUCESSO
// ================================
function mostrarModal() {
    const modal = document.getElementById("modalConfirmacao");

    modal.classList.add("ativo");
    setTimeout(() => modal.classList.remove("ativo"), 2000);
}

// ================================
//  INICIAR
// ================================
carregarAssignments();

