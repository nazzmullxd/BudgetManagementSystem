// Main application JavaScript functions

// Global state management
let currentPage = 'dashboard';
let appData = {
    expenses: [],
    incomes: [],
    categories: [],
    budgetGoals: []
};

// Navigation functions
function navigateTo(page, params = {}) {
    // Update URL without page reload
    history.pushState({ page, params }, '', `/${page}`);
    
    // Update active nav item
    $('.sidebar .nav-link').removeClass('active');
    $(`.sidebar .nav-link[onclick*="${page}"]`).addClass('active');
    
    currentPage = page;
    loadPage(page, params);
}

function loadPage(page, params = {}) {
    const mainContent = $('#main-content');
    
    showLoading();
    
    switch (page) {
        case 'dashboard':
            loadDashboard();
            break;
        case 'expenses':
            loadExpensesPage(params);
            break;
        case 'income':
            loadIncomePage(params);
            break;
        case 'budgets':
            loadBudgetsPage();
            break;
        case 'categories':
            loadCategoriesPage();
            break;
        case 'reports':
            loadReportsPage();
            break;
        case 'login':
            loadLoginPage();
            break;
        case 'register':
            loadRegisterPage();
            break;
        default:
            loadDashboard();
    }
    
    hideLoading();
}

function loadDashboard() {
    // Dashboard is loaded by default, just ensure data is refreshed
    if (isAuthenticated()) {
        loadDashboardData();
    }
}

function loadExpensesPage(params) {
    const content = `
        <div class="d-flex justify-content-between align-items-center mb-4">
            <h2>Expenses</h2>
            <button class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#expenseModal" onclick="openExpenseModal()">
                <i class="bi bi-plus-lg me-2"></i>Add Expense
            </button>
        </div>
        
        <div class="card">
            <div class="card-body">
                <div class="row mb-3">
                    <div class="col-md-4">
                        <input type="text" class="form-control" id="expense-search" placeholder="Search expenses...">
                    </div>
                    <div class="col-md-3">
                        <select class="form-select" id="expense-category-filter">
                            <option value="">All Categories</option>
                        </select>
                    </div>
                    <div class="col-md-2">
                        <input type="date" class="form-control" id="expense-date-from">
                    </div>
                    <div class="col-md-2">
                        <input type="date" class="form-control" id="expense-date-to">
                    </div>
                    <div class="col-md-1">
                        <button class="btn btn-outline-secondary" onclick="filterExpenses()">
                            <i class="bi bi-funnel"></i>
                        </button>
                    </div>
                </div>
                
                <div class="table-responsive">
                    <table class="table table-hover">
                        <thead class="table-light">
                            <tr>
                                <th>Item</th>
                                <th>Category</th>
                                <th>Amount</th>
                                <th>Quantity</th>
                                <th>Total</th>
                                <th>Date</th>
                                <th>Actions</th>
                            </tr>
                        </thead>
                        <tbody id="expenses-table-body">
                            <tr>
                                <td colspan="7" class="text-center py-4 text-muted">
                                    <i class="bi bi-hourglass-split me-2"></i>Loading expenses...
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        
        <!-- Add/Edit Expense Modal -->
        <div class="modal fade" id="expenseModal" tabindex="-1">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="expenseModalLabel">Add Expense</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                    </div>
                    <div class="modal-body">
                        <form id="expenseForm">
                            <div class="mb-3">
                                <label for="itemName" class="form-label">Item Name</label>
                                <input type="text" class="form-control" id="itemName" required>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <label for="itemPrice" class="form-label">Price</label>
                                    <div class="input-group">
                                        <span class="input-group-text">$</span>
                                        <input type="number" class="form-control" id="itemPrice" step="0.01" required>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <label for="quantity" class="form-label">Quantity</label>
                                    <input type="number" class="form-control" id="quantity" value="1" min="1" required>
                                </div>
                            </div>
                            <div class="mb-3">
                                <label for="expenseCategoryId" class="form-label">Category</label>
                                <select class="form-select" id="expenseCategoryId" required>
                                    <option value="">Select Category</option>
                                </select>
                            </div>
                            <div class="mb-3">
                                <label for="transactionDate" class="form-label">Date</label>
                                <input type="date" class="form-control" id="transactionDate" required>
                            </div>
                        </form>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                        <button type="button" class="btn btn-primary" onclick="saveExpense()">Save Expense</button>
                    </div>
                </div>
            </div>
        </div>
    `;
    
    $('#main-content').html(content);
    
    // Set default date to today
    $('#transactionDate').val(new Date().toISOString().split('T')[0]);
    
    // Load expenses and categories
    loadExpenses();
    loadCategoriesForDropdown();
}

function loadIncomePage(params) {
    const content = `
        <div class="d-flex justify-content-between align-items-center mb-4">
            <h2>Income</h2>
            <button class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#incomeModal" onclick="openIncomeModal()">
                <i class="bi bi-plus-lg me-2"></i>Add Income
            </button>
        </div>
        
        <div class="card">
            <div class="card-body">
                <div class="table-responsive">
                    <table class="table table-hover">
                        <thead class="table-light">
                            <tr>
                                <th>Source</th>
                                <th>Type</th>
                                <th>Description</th>
                                <th>Amount</th>
                                <th>Tax</th>
                                <th>Net Amount</th>
                                <th>Date</th>
                                <th>Actions</th>
                            </tr>
                        </thead>
                        <tbody id="income-table-body">
                            <tr>
                                <td colspan="8" class="text-center py-4 text-muted">
                                    <i class="bi bi-hourglass-split me-2"></i>Loading income...
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        
        <!-- Add/Edit Income Modal -->
        <div class="modal fade" id="incomeModal" tabindex="-1">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Add Income</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                    </div>
                    <div class="modal-body">
                        <form id="incomeForm">
                            <div class="mb-3">
                                <label for="incomeSource" class="form-label">Income Source</label>
                                <input type="text" class="form-control" id="incomeSource" required>
                            </div>
                            <div class="mb-3">
                                <label for="incomeType" class="form-label">Income Type</label>
                                <select class="form-select" id="incomeType" required>
                                    <option value="Salary">Salary</option>
                                    <option value="Freelance">Freelance</option>
                                    <option value="Investment">Investment</option>
                                    <option value="Business">Business</option>
                                    <option value="Other">Other</option>
                                </select>
                            </div>
                            <div class="mb-3">
                                <label for="incomeDescription" class="form-label">Description</label>
                                <input type="text" class="form-control" id="incomeDescription" required>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <label for="incomeAmount" class="form-label">Amount</label>
                                    <div class="input-group">
                                        <span class="input-group-text">$</span>
                                        <input type="number" class="form-control" id="incomeAmount" step="0.01" required>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <label for="incomeTax" class="form-label">Tax</label>
                                    <div class="input-group">
                                        <span class="input-group-text">$</span>
                                        <input type="number" class="form-control" id="incomeTax" step="0.01" value="0">
                                    </div>
                                </div>
                            </div>
                            <div class="mb-3">
                                <label for="incomeDate" class="form-label">Date</label>
                                <input type="date" class="form-control" id="incomeDate" required>
                            </div>
                            <div class="mb-3">
                                <label for="frequency" class="form-label">Frequency</label>
                                <select class="form-select" id="frequency">
                                    <option value="OneTime">One Time</option>
                                    <option value="Weekly">Weekly</option>
                                    <option value="Monthly">Monthly</option>
                                    <option value="Yearly">Yearly</option>
                                </select>
                            </div>
                        </form>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                        <button type="button" class="btn btn-primary" onclick="saveIncome()">Save Income</button>
                    </div>
                </div>
            </div>
        </div>
    `;
    
    $('#main-content').html(content);
    
    // Set default date to today
    $('#incomeDate').val(new Date().toISOString().split('T')[0]);
    
    // Load income data
    loadIncomes();
}

// Utility functions
function showLoading() {
    $('#loading').addClass('show');
}

function hideLoading() {
    $('#loading').removeClass('show');
}

function showSuccess(message) {
    showToast(message, 'success');
}

function showError(message) {
    showToast(message, 'danger');
}

function showInfo(message) {
    showToast(message, 'info');
}

function showToast(message, type = 'info') {
    const toastHtml = `
        <div class="toast align-items-center text-bg-${type} border-0 position-fixed top-0 end-0 m-3" role="alert" style="z-index: 9999;">
            <div class="d-flex">
                <div class="toast-body">
                    ${message}
                </div>
                <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
            </div>
        </div>
    `;
    
    $('body').append(toastHtml);
    const toast = $('.toast:last')[0];
    const bootstrapToast = new bootstrap.Toast(toast, { delay: 5000 });
    bootstrapToast.show();
    
    // Remove toast element after it's hidden
    $(toast).on('hidden.bs.toast', function() {
        $(this).remove();
    });
}

function formatCurrency(amount) {
    return new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD',
        minimumFractionDigits: 2
    }).format(amount);
}

function formatDate(date) {
    return new Date(date).toLocaleDateString('en-US', {
        year: 'numeric',
        month: 'short',
        day: 'numeric'
    });
}

// Data loading functions
function loadExpenses() {
    APIService.getExpenses(function(expenses) {
        appData.expenses = expenses;
        displayExpenses(expenses);
    }, function(xhr) {
        showError('Failed to load expenses');
        console.error('Failed to load expenses:', xhr);
    });
}

function loadIncomes() {
    APIService.getIncomes(function(incomes) {
        appData.incomes = incomes;
        displayIncomes(incomes);
    }, function(xhr) {
        showError('Failed to load incomes');
        console.error('Failed to load incomes:', xhr);
    });
}

function loadCategoriesForDropdown() {
    APIService.getCategories(function(categories) {
        appData.categories = categories;
        
        // Populate category dropdowns
        const categorySelects = $('#expenseCategoryId, #expense-category-filter');
        categorySelects.empty();
        
        $('#expenseCategoryId').append('<option value="">Select Category</option>');
        $('#expense-category-filter').append('<option value="">All Categories</option>');
        
        categories.forEach(category => {
            const option = `<option value="${category.expenseCategoryId}">${category.categoryName}</option>`;
            categorySelects.append(option);
        });
    }, function(xhr) {
        showError('Failed to load categories');
    });
}

function displayExpenses(expenses) {
    const tbody = $('#expenses-table-body');
    tbody.empty();
    
    if (expenses.length === 0) {
        tbody.html('<tr><td colspan="7" class="text-center py-4 text-muted"><i class="bi bi-inbox me-2"></i>No expenses found</td></tr>');
        return;
    }
    
    expenses.forEach(expense => {
        const total = expense.itemPrice * expense.quantity;
        tbody.append(`
            <tr class="fade-in">
                <td>${expense.itemName}</td>
                <td><span class="badge bg-light text-dark">${expense.expenseCategoryId}</span></td>
                <td>${formatCurrency(expense.itemPrice)}</td>
                <td>${expense.quantity}</td>
                <td class="fw-bold">${formatCurrency(total)}</td>
                <td>${formatDate(expense.transactionDate)}</td>
                <td>
                    <button class="btn btn-sm btn-outline-primary me-1" onclick="editExpense('${expense.expenseId}')">
                        <i class="bi bi-pencil"></i>
                    </button>
                    <button class="btn btn-sm btn-outline-danger" onclick="deleteExpense('${expense.expenseId}')">
                        <i class="bi bi-trash"></i>
                    </button>
                </td>
            </tr>
        `);
    });
}

function displayIncomes(incomes) {
    const tbody = $('#income-table-body');
    tbody.empty();
    
    if (incomes.length === 0) {
        tbody.html('<tr><td colspan="8" class="text-center py-4 text-muted"><i class="bi bi-inbox me-2"></i>No income found</td></tr>');
        return;
    }
    
    incomes.forEach(income => {
        const netAmount = income.incomeAmount - income.incomeTax;
        tbody.append(`
            <tr class="fade-in">
                <td>${income.incomeSource}</td>
                <td><span class="badge bg-info">${income.incomeType}</span></td>
                <td>${income.incomeDescription}</td>
                <td class="fw-bold text-success">${formatCurrency(income.incomeAmount)}</td>
                <td class="text-danger">${formatCurrency(income.incomeTax)}</td>
                <td class="fw-bold">${formatCurrency(netAmount)}</td>
                <td>${formatDate(income.incomeDate)}</td>
                <td>
                    <button class="btn btn-sm btn-outline-primary me-1" onclick="editIncome('${income.incomeId}')">
                        <i class="bi bi-pencil"></i>
                    </button>
                    <button class="btn btn-sm btn-outline-danger" onclick="deleteIncome('${income.incomeId}')">
                        <i class="bi bi-trash"></i>
                    </button>
                </td>
            </tr>
        `);
    });
}

// Modal functions
function openExpenseModal(expenseId = null) {
    $('#expenseModalLabel').text(expenseId ? 'Edit Expense' : 'Add Expense');
    $('#expenseForm')[0].reset();
    $('#transactionDate').val(new Date().toISOString().split('T')[0]);
    
    if (expenseId) {
        // Load expense data for editing
        const expense = appData.expenses.find(e => e.expenseId === expenseId);
        if (expense) {
            $('#itemName').val(expense.itemName);
            $('#itemPrice').val(expense.itemPrice);
            $('#quantity').val(expense.quantity);
            $('#expenseCategoryId').val(expense.expenseCategoryId);
            $('#transactionDate').val(expense.transactionDate.split('T')[0]);
        }
    }
}

function openIncomeModal(incomeId = null) {
    $('#incomeForm')[0].reset();
    $('#incomeDate').val(new Date().toISOString().split('T')[0]);
    
    if (incomeId) {
        // Load income data for editing
        const income = appData.incomes.find(i => i.incomeId === incomeId);
        if (income) {
            $('#incomeSource').val(income.incomeSource);
            $('#incomeType').val(income.incomeType);
            $('#incomeDescription').val(income.incomeDescription);
            $('#incomeAmount').val(income.incomeAmount);
            $('#incomeTax').val(income.incomeTax);
            $('#incomeDate').val(income.incomeDate.split('T')[0]);
            $('#frequency').val(income.frequency);
        }
    }
}

// Save functions
function saveExpense() {
    const formData = {
        itemName: $('#itemName').val(),
        itemPrice: parseFloat($('#itemPrice').val()),
        quantity: parseInt($('#quantity').val()),
        expenseCategoryId: $('#expenseCategoryId').val(),
        transactionDate: $('#transactionDate').val(),
        currencyId: 'USD' // Default currency
    };
    
    APIService.createExpense(formData, function(response) {
        showSuccess('Expense added successfully!');
        $('#expenseModal').modal('hide');
        loadExpenses(); // Reload expenses
    }, function(xhr) {
        showError('Failed to save expense. Please try again.');
    });
}

function saveIncome() {
    const formData = {
        incomeSource: $('#incomeSource').val(),
        incomeType: $('#incomeType').val(),
        incomeDescription: $('#incomeDescription').val(),
        incomeAmount: parseFloat($('#incomeAmount').val()),
        incomeTax: parseFloat($('#incomeTax').val()) || 0,
        incomeDate: $('#incomeDate').val(),
        frequency: $('#frequency').val(),
        currencyId: 'USD' // Default currency
    };
    
    APIService.createIncome(formData, function(response) {
        showSuccess('Income added successfully!');
        $('#incomeModal').modal('hide');
        loadIncomes(); // Reload incomes
    }, function(xhr) {
        showError('Failed to save income. Please try again.');
    });
}

// Handle browser navigation
window.addEventListener('popstate', function(event) {
    if (event.state && event.state.page) {
        loadPage(event.state.page, event.state.params || {});
    }
});

// Initialize app on document ready
$(document).ready(function() {
    // Initialize navigation based on current URL
    const path = window.location.pathname.substring(1) || 'dashboard';
    navigateTo(path);
});
