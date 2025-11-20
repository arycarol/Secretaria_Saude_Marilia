const app = Vue.createApp({
    data() {
        return {
            isSidebarActive: false,
        };
    }
});

app.mount("#app");

function getCookie(name) {
  const cookies = document.cookie.split(";").map(c => c.trim());
  for (const cookie of cookies) {
      if (cookie.startsWith(name + "=")) {
          return cookie.substring(name.length + 1);
      }
  }
  return null;
}

const token = getCookie("token");
const driverUserId = getCookie("driverUserId");

if (!token || !driverUserId) {
  alert("Sessão expirada. Faça login novamente.");
  window.location.href = "../login/login.html";
}

const API_BASE = "https://localhost:7041/api/v1";

const solicitacoesContainer = document.querySelector(".lista-solicitacoes");
const aceitasContainer = document.querySelector(".lista-aceitas");
const agendaContainer = document.querySelector(".lista-agenda");

async function carregarAssignments() {
  try {
      const response = await fetch(`${API_BASE}/TransportAssignment/GetListByDriverId/${driverUserId}`, {
          headers: { "Authorization": `Bearer ${token}` }
      });

      if (!response.ok) throw new Error("Erro ao carregar assignments");

      const assignments = await response.json();

      solicitacoesContainer.innerHTML = "";
      aceitasContainer.innerHTML = "";
      
      let solicitacoesPendentes = 0;
      let solicitacoesAceitas = 0;

      for (const item of assignments) {
          const paciente = await carregarPaciente(item.pacientUserId);
          const request = await carregarRequest(item.transportRequestId);

          if (item.transportAssignmentStatus === "Aguardando") {
              adicionarSolicitacaoPendente(
                  item.id,
                  request.hour,
                  paciente.name,
                  paciente.cpf,
                  request.transportKind,
                  request.originLocation,
                  request.destinationLocation
              );
              solicitacoesPendentes++;
          }

          if (item.transportAssignmentStatus === "Concluido" ||
              item.transportAssignmentStatus === "Aceito") {
              adicionarSolicitacaoAceita(
                  request.hour,
                  paciente.name,
                  paciente.cpf,
                  request.transportKind,
                  request.originLocation,
                  request.destinationLocation
              );
              solicitacoesAceitas++;
          }
      }
      
      if (solicitacoesPendentes === 0) {
        solicitacoesContainer.innerHTML = `<p class="mensagem-vazia">Nenhuma solicitação pendente.</p>`;
      }
      if (solicitacoesAceitas === 0) {
        aceitasContainer.innerHTML = `<p class="mensagem-vazia">Nenhuma solicitação aceita.</p>`;
      }


  } catch (err) {
      console.error(err);
      solicitacoesContainer.innerHTML = `<p class="mensagem-vazia">Erro ao carregar solicitações.</p>`;
      aceitasContainer.innerHTML = `<p class="mensagem-vazia">Erro ao carregar aceitas.</p>`;
  }
}

async function carregarAgenda() {
  try {
      const response = await fetch(`${API_BASE}/TransportAssignment/GetListOfToday/${driverUserId}`, {
          headers: { "Authorization": `Bearer ${token}` }
      });

      if (!response.ok) throw new Error("Erro ao carregar agenda");

      const assignments = await response.json();

      agendaContainer.innerHTML = "";
      
      if (assignments.length === 0) {
        agendaContainer.innerHTML = `<p class="mensagem-vazia">Nenhum agendamento para hoje.</p>`;
        return;
      }

      const concluidos = assignments.filter(a =>
          a.transportAssignmentStatus.toLowerCase() === "concluido"
      );

      for (const item of concluidos) {
          const paciente = await carregarPaciente(item.pacientUserId);
          const request = await carregarRequest(item.transportRequestId);

          adicionarAgenda(
              request.hour,
              paciente.name,
              paciente.cpf,
              request.transportKind,
              request.originLocation,
              request.destinationLocation
          );
      }

  } catch (err) {
      console.error(err);
      agendaContainer.innerHTML = `<p class="mensagem-vazia">Erro ao carregar agenda.</p>`;
  }
}

async function carregarPaciente(id) {
  const res = await fetch(`${API_BASE}/User/${id}`, {
      headers: { "Authorization": `Bearer ${token}` }
  });
  return await res.json();
}

async function carregarRequest(id) {
  const res = await fetch(`${API_BASE}/TransportRequest/${id}`, {
      headers: { "Authorization": `Bearer ${token}` }
  });
  return await res.json();
}

function adicionarSolicitacaoPendente(id, hora, nome, cpf, tipoAtendimento, origem, destino) {
  const div = document.createElement("div");
  div.classList.add("box-info");

  div.innerHTML = `
      <p><strong>${hora} - ${nome} - CPF: ${cpf}</strong></p>

      <p><strong>TIPO:</strong> ${tipoAtendimento}</p>

      <p><strong>PARTIDA:</strong> ${origem}</p>
      <p><strong>DESTINO:</strong> ${destino}</p>

      <div class="botoes">
        <button class="btn-recusar">RECUSAR</button>
        <button class="btn-aceitar">ACEITAR</button>
      </div>
  `;

  div.querySelector(".btn-aceitar")
     .addEventListener("click", () => aceitarSolicitacao(id, div));

  solicitacoesContainer.appendChild(div);
}

function adicionarSolicitacaoAceita(hora, nome, cpf, tipoAtendimento, origem, destino) {
  const div = document.createElement("div");
  div.classList.add("box-info");

  div.innerHTML = `
      <p><strong>${hora} - ${nome} - CPF: ${cpf}</strong></p>

      <p><strong>TIPO:</strong> ${tipoAtendimento}</p>

      <p><strong>PARTIDA:</strong> ${origem}</p>
      <p><strong>DESTINO:</strong> ${destino}</p>
  `;

  aceitasContainer.appendChild(div);
}

function adicionarAgenda(hora, nome, cpf, tipoAtendimento, origem, destino) {
  const div = document.createElement("div");
  div.classList.add("box-agenda");

  div.innerHTML = `
      <p><strong>${hora} - ${nome} - CPF: ${cpf}</strong></p>

      <p><strong>TIPO:</strong> ${tipoAtendimento}</p>

      <p><strong>PARTIDA:</strong> ${origem}</p>
      <p><strong>DESTINO:</strong> ${destino}</p>

      <span class="status-bolinha ainda"></span>
  `;

  agendaContainer.appendChild(div);
}

async function aceitarSolicitacao(id, elemento) {
  try {
      const response = await fetch(`${API_BASE}/TransportAssignment/Accept/${id}`, {
          method: "POST",
          headers: { "Authorization": `Bearer ${token}` }
      });

      if (!response.ok) throw new Error("Erro ao aceitar solicitação");

      elemento.remove();
      mostrarModal();
      await carregarAssignments();
      await carregarAgenda();

  } catch (err) {
      console.error(err);
      alert("Falha ao aceitar a solicitação. Tente novamente.");
  }
}

function mostrarModal() {
  const modal = document.getElementById("modalConfirmacao");
  modal.classList.add("ativo");
  setTimeout(() => modal.classList.remove("ativo"), 1800);
}

carregarAssignments();
carregarAgenda();