// Variables globales
let products = [];
let categories = [];
let suppliers = [];
let currentProductId = null;

// elementos
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

// filtros
const searchInput = document.getElementById('search');
const categoryFilter = document.getElementById('category');
const supplierFilter = document.getElementById('supplier');
const statusFilter = document.getElementById('status-input');
const minPriceInput = document.getElementById('min-price-input');
const maxPriceInput = document.getElementById('max-price-input');
const stockInput = document.getElementById('stock-input');
const applyFiltersBtn = document.getElementById('btn-apply-filters');
const resetFiltersBtn = document.getElementById('btn-reset-filters');

// formulario
const productIdInput = document.getElementById('product-id');
const productNameInput = document.getElementById('product-name');
const productDescInput = document.getElementById('product-description');
const productPriceInput = document.getElementById('product-price');
const productStockInput = document.getElementById('product-stock');
const productCategorySelect = document.getElementById('product-category');
const productSupplierSelect = document.getElementById('product-supplier');
const productExpirationInput = document.getElementById('product-expiration');
const productStatusSelect = document.getElementById('product-status');

// Inicialización cuando el DOM está listo
document.addEventListener('DOMContentLoaded', () => {
    init();
    setupEventListeners();
});

// Inicializar la aplicación
async function init() {
    try {
        // Cargar datos necesarios
        await Promise.all([
            fetchCategories(),
            fetchSuppliers(),
            fetchProducts()
        ]);
        
        // crea los inputs de categorías y proveedores
        createCategorySelects();
        createSupplierSelects();
        
        // se muestran los productos
        renderProducts(products);
    } catch (error) {
        showError('Error al inicializar la aplicación: ' + error.message);
    }
}

// Configurar todos los event listeners
function setupEventListeners() {
    // Navegación
    hamburgerMenu.addEventListener('click', toggleSidebar);
    
    // Filtros
    applyFiltersBtn.addEventListener('click', applyFilters);
    resetFiltersBtn.addEventListener('click', resetFilters);
    
    // Modal y formulario
    addProductBtn.addEventListener('click', openAddProductModal);
    closeModalBtn.addEventListener('click', closeModal);
    cancelFormBtn.addEventListener('click', closeModal);
    productForm.addEventListener('submit', handleProductSubmit);
    
    // Cierre del modal al hacer clic fuera
    productModal.addEventListener('click', (e) => {
        if (e.target === productModal) {
            closeModal();
        }
    });
}


// Obtener productos desde la API
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

// Obtener productos con filtros seleccionados
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

// Obtener categorías desde la API
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

// Obtener proveedores desde la API
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

// Rellenar los selectores de categorías (rellenar el selector de filtro y el del formulario)
function createCategorySelects() {
    categories.forEach(category => {
        categoryFilter.innerHTML += `<option value="${category.categoryId}">${category.name}</option>`;
    });
    
    productCategorySelect.innerHTML = '';
    categories.forEach(category => {
        productCategorySelect.innerHTML += `<option value="${category.categoryId}">${category.name}</option>`;
    });
}

// Rellenar los selectores de proveedores (rellenar el selector de filtro y el del formulario)
function createSupplierSelects() {
    suppliers.forEach(supplier => {
        supplierFilter.innerHTML += `<option value="${supplier.supplierId}">${supplier.name}</option>`;
    });
    
    productSupplierSelect.innerHTML = '';
    suppliers.forEach(supplier => {
        productSupplierSelect.innerHTML += `<option value="${supplier.supplierId}">${supplier.name}</option>`;
    });
}


// Renderizar productos en la página
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
        
        // Añadir event listeners a los botones
        productCard.querySelector('.btn-edit').addEventListener('click', () => openEditProductModal(product.productId));
        productCard.querySelector('.btn-delete').addEventListener('click', () => confirmDeleteProduct(product.productId, product.name));
        
        productsContainer.appendChild(productCard);
    });
}

// Obtener texto del estado
function getStatusText(status) {
    switch(status) {
        case 'R': return 'Registrado';
        case 'A': return 'Anulado';
    }
}

// Obtener clase CSS para el estado
function getStatusClass(status) {
    switch(status) {
        case 'R': return 'active';
        case 'A': return 'inactive';
    }
}

// Formatear fecha
function formatDate(dateString) {
    const date = new Date(dateString);
    return date.toLocaleDateString();
}


// Aplicar filtros
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

        // actualiza la vista con los productos filtrados
        renderProducts(products);

    } catch (error) {
        showError('Error al aplicar filtros: ' + error.message);
        renderProducts([]);
    } finally {
        hideLoader();
        // para móviles, cerrar el sidebar después de aplicar filtros
        if (window.innerWidth <= 768) {
            toggleSidebar();
        }
    }
}

// valida si los filtros se aplicaron correctamente
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

// Resetear filtros
function resetFilters() {
    searchInput.value = '';
    categoryFilter.value = '';
    supplierFilter.value = '';
    statusFilter.value = 'R';
    minPriceInput.value = '';
    maxPriceInput.value = '';
    stockInput.value = '';
    
    renderProducts(products);
}


// Abrir modal para añadir producto
function openAddProductModal() {
    modalTitle.textContent = 'Añadir Producto';
    resetProductForm();
    currentProductId = null;
    openModal();
}

// Abrir modal para editar producto
function openEditProductModal(productId) {
    const product = products.find(p => p.productId === productId);
    if (!product) {
        showError('Producto no encontrado');
        return;
    }
    
    modalTitle.textContent = 'Editar Producto';
    currentProductId = productId;
    
    // Rellenar el formulario con los datos del producto
    productIdInput.value = product.productId;
    productNameInput.value = product.name;
    productDescInput.value = product.description;
    productPriceInput.value = product.price;
    productStockInput.value = product.stock;
    productCategorySelect.value = product.categoryId;
    productSupplierSelect.value = product.supplierId;
    productStatusSelect.value = product.status;
    
    if (product.expirationDate) {
        // Convertir la fecha a formato YYYY-MM-DD para el input date
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

// Manejar el envío del formulario (crear o actualizar)
async function handleProductSubmit(event) {
    event.preventDefault();
    
    // Recoger datos del formulario
    const productData = {
        name: productNameInput.value,
        description: productDescInput.value,
        price: parseFloat(productPriceInput.value),
        stock: parseInt(productStockInput.value),
        categoryId: productCategorySelect.value,
        supplierId: productSupplierSelect.value,
        status: productStatusSelect.value,
        expirationDate: productExpirationInput.value || null
    };
    
    try {
        // Si hay un id seleccionado se procede a actualizar, sino a crear el producto
        if (currentProductId) {
            await updateProduct(productData);
            showSuccess('Producto actualizado correctamente');
        } else {
            await createProduct(productData);
            showSuccess('Producto creado correctamente');
        }
        
        // Recargar productos y cerrar modal
        resetFilters();
        await fetchProducts();
        renderProducts(products);
        closeModal();
    } catch (error) {
        showError('Error al guardar el producto: ' + error.message);
    }
}

// Crear nuevo producto
async function createProduct(productData) {
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
    
    return await response.json();
}

// Actualizar producto existente
async function updateProduct(productData) {
    const response = await fetch(`/products/${currentProductId}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(productData)
    });
    
    if (!response.ok) {
        throw new Error('Error al actualizar el producto');
    }
    
    return await response.json();
}

// Confirmar eliminación de producto
function confirmDeleteProduct(productId, productName) {
    if (confirm(`¿Estás seguro de que quieres eliminar el producto "${productName}"?`)) {
        deleteProduct(productId);
    }
}

// Eliminar producto
async function deleteProduct(productId) {
    try {
        const response = await fetch(`/products/${productId}`, {
            method: 'DELETE'
        });
        
        if (!response.ok) {
            throw new Error('Error al eliminar el producto');
        }
        
        // Recargar productos después de eliminar
        await fetchProducts();
        renderProducts(products);
        showSuccess('Producto eliminado correctamente');
    } catch (error) {
        showError('Error al eliminar el producto: ' + error.message);
    }
}


// Resetear formulario de producto
function resetProductForm() {
    productForm.reset();
    productIdInput.value = '';
}

// Abrir modal
function openModal() {
    productModal.classList.add('active');
    document.body.style.overflow = 'hidden'; // Evitar scroll de fondo
}

// Cerrar modal
function closeModal() {
    productModal.classList.remove('active');
    document.body.style.overflow = '';
}

// Toggle sidebar (para móvil)
function toggleSidebar() {
    sidebar.classList.toggle('active');
    hamburgerMenu.classList.toggle('active');
}

// Mostrar loader
function showLoader() {
    loader.style.display = 'flex';
}

// Ocultar loader
function hideLoader() {
    loader.style.display = 'none';
}

// Mostrar mensaje de error
function showError(message) {
    //alert(message); // Se podría mejorar con un sistema de notificaciones personalizado
}

// Mostrar mensaje de éxito
function showSuccess(message) {
    //alert(message); // Se podría mejorar con un sistema de notificaciones personalizado
}