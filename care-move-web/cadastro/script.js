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

const telefone = document.getElementById('telefone');

telefone.addEventListener('input', (e) => {
  let valor = e.target.value.replace(/\D/g, '');
  if (valor.length > 11) valor = valor.slice(0, 11);
  valor = valor.replace(/^(\d{2})(\d)/g, '($1) $2');
  valor = valor.replace(/(\d{5})(\d{1,4})$/, '$1-$2');
  e.target.value = valor;
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

document.getElementById("cadastro").addEventListener("click", cadastrarUser);

function cadastrarUser(event) {
    event.preventDefault();

    const nome = document.querySelector('input[placeholder="Nome Completo"]').value;
    const email = document.querySelector('input[placeholder="E-mail"]').value;
    const cpf = document.getElementById("cpf").value.replace(/\D/g, '');
    const telefone = document.getElementById("telefone").value;
    const dataNasc = document.querySelector('input[type="date"]').value;
    const senha = document.querySelectorAll('input[type="password"]')[0].value;
    const confirmar = document.querySelectorAll('input[type="password"]')[1].value;

    if (!nome || !email || !cpf || !telefone || !dataNasc || !senha) {
        alert("Preencha todos os campos!");
        return;
    }

    if (senha !== confirmar) {
        alert("As senhas não coincidem!");
        return;
    }

    const birthDate = dataNasc;

    const usuario = {
        name: nome,
        cpf: cpf,
        email: email,
        telephone: telefone,
        birthDate: birthDate,
        password: senha,
        userCategory: "Paciente",
        vehicleId: null
    };

    console.log("JSON enviado:", usuario);

    fetch("http://localhost:5260/api/v1/user", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(usuario)
    })
    .then(async response => {
        const data = await response.json();

        if (!response.ok) {
            throw new Error(data.message || "Erro ao cadastrar usuário");
        }

        alert("Usuário cadastrado com sucesso!");

        setTimeout(() => {
            window.location.href = "../login/login.html";
        }, 1000);
    })
    .catch(error => {
        console.error("Erro:", error);
        alert("Erro ao cadastrar: " + error.message);
    });
}
