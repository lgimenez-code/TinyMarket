// global variables
let products = [];
let categories = [];
let suppliers = [];
let currentProductId = null;

// elements
const productsContainer = document.getElementById('products-container');
const loader = document.getElementById('loader');
const sidebar = document.getElementById('sidebar');
const hamburgerMenu = document.getElementById('hamburger-menu');
const productModal = document.getElementById('product-modal');
const productForm = document.getElementById('product-form');
const modalTitle = document.getElementById('modal-title');
const closeModalBtn = document.getElementById('close-modal');
const cancelFormBtn = document.getElementById('cancel-form');
const addProductBtn = document.getElementById('add-product');

// filters
const searchInput = document.getElementById('search');
const categoryFilter = document.getElementById('category');
const supplierFilter = document.getElementById('supplier');
const statusFilter = document.getElementById('status-input');
const minPriceInput = document.getElementById('min-price-input');
const maxPriceInput = document.getElementById('max-price-input');
const stockInput = document.getElementById('stock-input');
const applyFiltersBtn = document.getElementById('btn-apply-filters');
const resetFiltersBtn = document.getElementById('btn-reset-filters');

// form inputs
const productIdInput = document.getElementById('product-id');
const productNameInput = document.getElementById('product-name');
const productDescInput = document.getElementById('product-description');
const productPriceInput = document.getElementById('product-price');
const productStockInput = document.getElementById('product-stock');
const productCategorySelect = document.getElementById('product-category');
const productSupplierSelect = document.getElementById('product-supplier');
const productExpirationInput = document.getElementById('product-expiration');
const productStatusSelect = document.getElementById('product-status');

// initialize the app and the events
document.addEventListener('DOMContentLoaded', () => {
    init();
    setupEventListeners();
});

// initialize the app
async function init() {
    try {
        // makes requests to fill in the form fields
        await Promise.all([
            fetchCategories(),
            fetchSuppliers(),
            fetchProducts()
        ]);

        // creates category and supplier inputs
        createCategorySelects();
        createSupplierSelects();

        // the products are displayed
        renderProducts(products);
    } catch (error) {
        showError('Error al inicializar la aplicación: ' + error.message);
    }
}

// configure all event listeners
function setupEventListeners() {
    hamburgerMenu.addEventListener('click', toggleSidebar);
    applyFiltersBtn.addEventListener('click', applyFilters);
    resetFiltersBtn.addEventListener('click', resetFilters);
    addProductBtn.addEventListener('click', openAddProductModal);
    closeModalBtn.addEventListener('click', closeModal);
    cancelFormBtn.addEventListener('click', closeModal);
    productForm.addEventListener('submit', handleProductSubmit);
    productModal.addEventListener('click', (e) => {
        if (e.target === productModal) {
            closeModal();
        }
    });
}


// get products from the API
async function fetchProducts() {
    showLoader();
    try {
        const response = await fetch('/products');
        if (!response.ok) {
            throw new Error('Error al cargar productos');
        }
        products = await response.json();
    } catch (error) {
        showError('Error al cargar productos: ' + error.message);
        products = [];
    } finally {
        hideLoader();
    }
}

// get products with selected filters
async function fetchProductsFiltered(filters) {
    showLoader();
    try {
        const response = await fetch('/products/filtered', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(filters)
        });
        if (!response.ok) {
            throw new Error('Error al cargar productos');
        }
        products = await response.json();

    } catch (error) {
        showError('Error al cargar productos: ' + error.message);
        products = [];
    } finally {
        hideLoader();
    }
}

// get categories from the API
async function fetchCategories() {
    try {
        const response = await fetch('/categories');
        if (!response.ok) {
            throw new Error('Error al cargar categorías');
        }
        categories = await response.json();
    } catch (error) {
        showError('Error al cargar las categorías: ' + error.message);
        categories = [];
    }
}

// get providers from the API
async function fetchSuppliers() {
    try {
        const response = await fetch('/suppliers');
        if (!response.ok) {
            throw new Error('Error al cargar los proveedores');
        }
        suppliers = await response.json();
    } catch (error) {
        showError('Error al cargar proveedores: ' + error.message);
        suppliers = [];
    }
}

// fill in the category selectors (fill in the filter and form selectors)
function createCategorySelects() {
    categories.forEach(category => {
        categoryFilter.innerHTML += `<option value="${category.categoryId}">${category.name}</option>`;
    });

    productCategorySelect.innerHTML = '';
    categories.forEach(category => {
        productCategorySelect.innerHTML += `<option value="${category.categoryId}">${category.name}</option>`;
    });
}

// fill in the supplier selectors (fill in the filter selector and the form selector)
function createSupplierSelects() {
    suppliers.forEach(supplier => {
        supplierFilter.innerHTML += `<option value="${supplier.supplierId}">${supplier.name}</option>`;
    });

    productSupplierSelect.innerHTML = '';
    suppliers.forEach(supplier => {
        productSupplierSelect.innerHTML += `<option value="${supplier.supplierId}">${supplier.name}</option>`;
    });
}


// render products on the page
function renderProducts(productsToRender) {
    if (productsToRender.length === 0) {
        productsContainer.innerHTML = '<div class="no-products">No se encontraron productos</div>';
        return;
    }

    productsContainer.innerHTML = '';
    productsToRender.forEach(product => {
        const category = categories.find(c => c.categoryId === product.categoryId) || { name: 'Desconocida' };
        const supplier = suppliers.find(s => s.supplierId === product.supplierId) || { name: 'Desconocido' };

        const statusText = getStatusText(product.status);
        const statusClass = getStatusClass(product.status);

        const productCard = document.createElement('div');
        productCard.className = 'product-card';
        productCard.innerHTML = `
            <div class="product-header">
                <h3>${product.name}</h3>
                <span class="status-badge ${statusClass}">${statusText}</span>
            </div>
            <div class="product-details">
                <p class="product-description">${product.description}</p>
                <div class="product-info">
                    <p><strong>Precio:</strong> $${product.price.toFixed(2)}</p>
                    <p><strong>Stock:</strong> ${product.stock}</p>
                    <p><strong>Categoría:</strong> ${category.name}</p>
                    <p><strong>Proveedor:</strong> ${supplier.name}</p>
                    ${product.expirationDate ? `<p><strong>Expiración:</strong> ${formatDate(product.expirationDate)}</p>` : ''}
                </div>
            </div>
            <div class="product-actions">
                <button class="btn-edit btn-base" data-id="${product.productId}">Editar</button>
                <button class="btn-delete btn-base" data-id="${product.productId}">Eliminar</button>
            </div>
        `;

        // add Events to buttons
        productCard.querySelector('.btn-edit').addEventListener('click', () => openEditProductModal(product.productId));
        productCard.querySelector('.btn-delete').addEventListener('click', () => confirmDeleteProduct(product.productId, product.name));

        productsContainer.appendChild(productCard);
    });
}

// get the Status text based on the status code
function getStatusText(status) {
    switch(status) {
        case 'R': return 'Registrado';
        case 'A': return 'Anulado';
    }
}

// get the Status class based on the status code
function getStatusClass(status) {
    switch(status) {
        case 'R': return 'active';
        case 'A': return 'inactive';
    }
}

// format dates
function formatDate(dateString) {
    const date = new Date(dateString);
    return date.toLocaleDateString();
}


// apply the selected filters and then make a product request
async function applyFilters() {
    showLoader();
    if (validateFilters()) {
        return false;
    }

    try {
        const filterDTO = {
            Name: searchInput.value || null,
            MinPrice: minPriceInput.value ? parseFloat(minPriceInput.value) : null,
            MaxPrice: maxPriceInput.value ? parseFloat(maxPriceInput.value) : null,
            Stock: stockInput.value ? parseInt(stockInput.value) : null,
            CategoryId: categoryFilter.value || null,
            SupplierId: supplierFilter.value || null,
            Status: statusFilter.value || null
        };

        await fetchProductsFiltered(filterDTO);

        // refresh the view with the filtered products
        renderProducts(products);

    } catch (error) {
        showError('Error al aplicar filtros: ' + error.message);
        renderProducts([]);
    } finally {
        hideLoader();
        // for mobile devices, close the sidebar after applying filters
        if (window.innerWidth <= 768) {
            toggleSidebar();
        }
    }
}

// validates whether the filters were applied correctly
function validateFilters() {
    const fMinPrice = parseFloat(minPriceInput.value)
    const fMaxPrice = parseFloat(maxPriceInput.value)
    if (fMinPrice && fMaxPrice && (fMinPrice > fMaxPrice)) {
        minPriceInput.classList.add('inactive')
        maxPriceInput.classList.add('inactive')
        return true;
    }

    minPriceInput.classList.remove('inactive')
    maxPriceInput.classList.remove('inactive')
    return false;
}

// reset filters to default values
function resetFilters() {
    searchInput.value = '';
    categoryFilter.value = '';
    supplierFilter.value = '';
    statusFilter.value = 'R';
    minPriceInput.value = '';
    maxPriceInput.value = '';
    stockInput.value = '';
    minPriceInput.classList.remove('inactive')
    maxPriceInput.classList.remove('inactive')

    renderProducts(products);
}


// open modal to add a new product
function openAddProductModal() {
    modalTitle.textContent = 'Añadir Producto';
    productForm.reset();
    productIdInput.value = '';
    currentProductId = null;
    openModal();
}

// open modal to edit an existing product
function openEditProductModal(productId) {
    const product = products.find(p => p.productId === productId);
    if (!product) {
        showError('Producto no encontrado');
        return;
    }

    modalTitle.textContent = 'Editar Producto';
    currentProductId = productId;

    // fill in the form with product data
    productIdInput.value = product.productId;
    productNameInput.value = product.name;
    productDescInput.value = product.description;
    productPriceInput.value = product.price;
    productStockInput.value = product.stock;
    productCategorySelect.value = product.categoryId;
    productSupplierSelect.value = product.supplierId;
    productStatusSelect.value = product.status;

    if (product.expirationDate) {
        // convert expiration date to YYYY-MM-DD format
        const date = new Date(product.expirationDate);
        const year = date.getFullYear();
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const day = String(date.getDate()).padStart(2, '0');
        productExpirationInput.value = `${year}-${month}-${day}`;
    } else {
        productExpirationInput.value = '';
    }

    openModal();
}

// handle product form submission
async function handleProductSubmit(event) {
    event.preventDefault();

    // get form data
    const productData = {
        productId: currentProductId || null,
        name: productNameInput.value,
        description: productDescInput.value,
        price: parseFloat(productPriceInput.value),
        stock: parseInt(productStockInput.value),
        categoryId: parseInt(productCategorySelect.value),
        supplierId: parseInt(productSupplierSelect.value),
        status: productStatusSelect.value,
        expirationDate: productExpirationInput.value || null
    };

    try {
        // if a product ID is selected, it is updated, otherwise the product is created.
        if (currentProductId) {
            const response = await updateProduct(productData);
            showSuccess('Producto actualizado correctamente');
        } else {
            const response = await createProduct(productData);
            showSuccess('Producto creado correctamente');
        }
        closeModal();
        // reload products and close modal
        resetFilters();
        await fetchProducts();
        renderProducts(products);
        closeModal();
    } catch (error) {
        showError('Error al guardar el producto: ' + error.message);
    }
}

// create a new product
async function createProduct(productData) {
    try {
        const response = await fetch('/products', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(productData)
        });
        if (!response.ok) {
            throw new Error('Error al crear el producto');
        }

    } catch (error) {
        showError('Error al crear el producto: ' + error.message);
        return;        
    }
}

// update an existing product
async function updateProduct(productData) {
    try {
        const response = await fetch(`/products`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(productData)
        });

        if (!response.ok) {
            throw new Error('Error al actualizar el producto');
        }

    } catch (error ) {
        showError('Error al actualizar el producto: ' + error.message);
        return;
    }
}

// confirm product deletion
function confirmDeleteProduct(productId, productName) {
    if (confirm(`¿Estás seguro de que quieres eliminar el producto "${productName}"?`)) {
        deleteProduct(productId);
    }
}

// delete a product
async function deleteProduct(productId) {
    try {
        const response = await fetch(`/products/${productId}`, {
            method: 'DELETE'
        });

        if (!response.ok) {
            throw new Error('Error al eliminar el producto');
        }

        // reload products after deleting
        await fetchProducts();
        renderProducts(products);
        showSuccess('Producto eliminado correctamente');
    } catch (error) {
        showError('Error al eliminar el producto: ' + error.message);
    }
}

function openModal() {
    productModal.classList.add('activeModal');
    document.body.style.overflow = 'hidden'; // avoid background scrolling
}

function closeModal() {
    productModal.classList.remove('activeModal');
    document.body.style.overflow = '';
}

function toggleSidebar() {
    sidebar.classList.toggle('activePanel');
    hamburgerMenu.classList.toggle('activeButton');
}

function showLoader() {
    loader.style.display = 'flex';
}

function hideLoader() {
    loader.style.display = 'none';
}

function showError(message) {
    alert(message);
}

function showSuccess(message) {
    alert(message);
}