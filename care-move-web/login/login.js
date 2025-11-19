const toggleIcons = document.querySelectorAll('.input-group .show');

toggleIcons.forEach(icon => {
  icon.addEventListener('click', () => {
    const input = icon.previousElementSibling;
    if (input.type === 'password') {
      input.type = 'text';
      icon.classList.remove('fa-eye');
      icon.classList.add('fa-eye-slash');
    } else {
      input.type = 'password';
      icon.classList.remove('fa-eye-slash');
      icon.classList.add('fa-eye');
    }
  });
});

const cpf = document.getElementById('cpf');

cpf.addEventListener('input', (e) => {
  let valor = e.target.value.replace(/\D/g, '');
  if (valor.length > 11) valor = valor.slice(0, 11);
  valor = valor.replace(/^(\d{3})(\d)/, '$1.$2');
  valor = valor.replace(/^(\d{3})\.(\d{3})(\d)/, '$1.$2.$3');
  valor = valor.replace(/^(\d{3})\.(\d{3})\.(\d{3})(\d)/, '$1.$2.$3-$4');
  e.target.value = valor;
});

// 👁 Mostrar/Ocultar senha
const toggleIcon = document.querySelector('.input-group .show');
const senhaInput = document.getElementById('senha');

toggleIcon.addEventListener('click', () => {
    senhaInput.type = senhaInput.type === 'password' ? 'text' : 'password';
    toggleIcon.classList.toggle('fa-eye');
    toggleIcon.classList.toggle('fa-eye-slash');
});

// LOGIN
document.querySelector(".btn").addEventListener("click", async (event) => {
    event.preventDefault();

    const cpf = document.getElementById("cpf").value.replace(/\D/g, '');
    const password = document.getElementById("senha").value;

    if (!cpf || !password) {
        alert("Preencha CPF e senha!");
        return;
    }

    const loginData = { cpf, password };

    try {
        const response = await fetch("http://localhost:5260/api/Authenticate/Authenticate", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(loginData)
        });

        if (!response.ok) {
            alert("CPF ou senha incorretos!");
            return;
        }

        // AGORA O RETORNO É JSON:
        const result = await response.json();
        console.log("RETORNO LOGIN:", result);

        const token = result.token;
        const userCategory = result.userCategory;
        const userId = result.userId;

        // ================================
        // SALVAR COOKIES SEPARADOS
        // ================================
        document.cookie = `jwt=${token}; path=/; max-age=86400; samesite=strict`;
        document.cookie = `userCategory=${userCategory}; path=/; max-age=86400; samesite=strict`;
        document.cookie = `userId=${userId}; path=/; max-age=86400; samesite=strict`;

        // ================================
        // REDIRECIONAMENTO POR CATEGORIA
        // ================================
        if (userCategory === "Administrador" || userCategory === "Admin") {
            window.location.href = "../painel adm/index.html";
        } 
        
        else if (userCategory === "Paciente") {
            window.location.href = "../solicitações de transporte e agendamento/index.html";
        }

        else if (userCategory === "Motorista"){
            window.location.href = "../painel motorista/painel_motorista.html";
        }

    } catch (error) {
        console.error("Erro:", error);
        alert("Erro ao conectar com a API.");
    }
});
