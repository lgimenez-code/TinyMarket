

const hamburgerMenu = document.getElementById('hamburger-menu');
const addProductBtn = document.getElementById('add-product');
const productModal = document.getElementById('product-modal');



hamburgerMenu.addEventListener('click', () => {
    sidebar.classList.toggle('active');
});

addProductBtn.addEventListener('click', () => {
    document.getElementById('modal-title').textContent = 'Añadir Producto';
    document.getElementById('product-id').value = '';
    productModal.classList.add('active');
});