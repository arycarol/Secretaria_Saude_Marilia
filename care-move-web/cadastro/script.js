// Olhinho da senha
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

// Máscara de telefone
const telefone = document.getElementById('telefone');

telefone.addEventListener('input', (e) => {
  let valor = e.target.value.replace(/\D/g, '');
  if (valor.length > 11) valor = valor.slice(0, 11);
  valor = valor.replace(/^(\d{2})(\d)/g, '($1) $2');
  valor = valor.replace(/(\d{5})(\d{1,4})$/, '$1-$2');
  e.target.value = valor;
});

// Máscara de CPF
const cpf = document.getElementById('cpf');

cpf.addEventListener('input', (e) => {
  let valor = e.target.value.replace(/\D/g, '');
  if (valor.length > 11) valor = valor.slice(0, 11);
  valor = valor.replace(/^(\d{3})(\d)/, '$1.$2');
  valor = valor.replace(/^(\d{3})\.(\d{3})(\d)/, '$1.$2.$3');
  valor = valor.replace(/^(\d{3})\.(\d{3})\.(\d{3})(\d)/, '$1.$2.$3-$4');
  e.target.value = valor;
});
