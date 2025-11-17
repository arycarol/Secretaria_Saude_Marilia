const formEmail = document.getElementById('form-email');
const cardEmail = document.getElementById('card-email');
const cardCodigo = document.getElementById('card-codigo');
const brandText = document.getElementById('brand-text');
const resendLink = document.querySelector('.resend-link');
const codeInputs = document.querySelectorAll('.code-input');
const backArrowEmail = document.getElementById('back-email');
const backArrowCodigo = document.getElementById('back-codigo');

formEmail.addEventListener('submit', function(e) {
  e.preventDefault();
  const email = document.getElementById('email').value;

  brandText.querySelector('p').innerHTML = `
    Um código de verificação foi enviado para o seu e-mail.<br>
    Verifique sua caixa de entrada para prosseguir com o<br>
    processo de recuperação de senha.
  `;

  cardEmail.style.opacity = 0;
  cardEmail.style.pointerEvents = 'none';
  cardCodigo.classList.add('show');
  backArrowEmail.classList.add('hidden');
  backArrowCodigo.classList.remove('hidden');
});

resendLink.addEventListener('click', function(e) {
  e.preventDefault();
  const email = document.getElementById('email').value;

  if(email) {
    showModal(`Código reenviado para: ${email}`);
  } else {
    showModal('Digite seu e-mail primeiro!');
  }
});

codeInputs.forEach((input, idx) => {
  input.addEventListener('input', () => {
    if(input.value.length === 1 && idx < codeInputs.length - 1) codeInputs[idx+1].focus();
    if(input.value.length === 0 && idx > 0) codeInputs[idx-1].focus();
  });

  input.addEventListener('keypress', (e) => {
    if(!/[0-9]/.test(e.key)) e.preventDefault();
  });
});

const btnCodigo = cardCodigo.querySelector('.btn');
btnCodigo.addEventListener('click', () => {
  let codigo = '';
  codeInputs.forEach(input => codigo += input.value);

  if(codigo.length === 6) {
    showModal('Código enviado! (simulação)');
  } else {
    showModal('Digite os 6 dígitos do código.');
  }
});

function showModal(message) {
  const modal = document.getElementById("custom-modal");
  const modalMessage = document.getElementById("modal-message");
  const modalBtn = document.getElementById("modal-btn");

  modalMessage.textContent = message;
  modal.classList.add("show");

  modalBtn.onclick = function () {
    modal.classList.remove("show");
  };
}

backArrowCodigo.addEventListener("click", () => {
  cardCodigo.classList.remove("show");
  cardEmail.style.opacity = 1;
  cardEmail.style.pointerEvents = 'auto';
  backArrowCodigo.classList.add('hidden');
  backArrowEmail.classList.remove('hidden');
});

backArrowEmail.addEventListener("click", () => {
  window.location.href = "login.html";
});
