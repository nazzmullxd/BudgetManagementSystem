// API helper functions for making authenticated requests

class BudgetAPI {
    constructor() {
        this.baseURL = '/api';
        this.defaultHeaders = {
            'Content-Type': 'application/json'
        };
    }

    getAuthHeaders() {
        const token = localStorage.getItem('authToken');
        return token ? { 'Authorization': `Bearer ${token}` } : {};
    }

    async request(endpoint, options = {}) {
        const url = `${this.baseURL}${endpoint}`;
        const config = {
            ...options,
            headers: {
                ...this.defaultHeaders,
                ...this.getAuthHeaders(),
                ...options.headers
            }
        };

        try {
            const response = await fetch(url, config);
            
            if (response.status === 401) {
                logout();
                throw new Error('Authentication required');
            }
            
            if (!response.ok) {
                const errorData = await response.json().catch(() => ({}));
                throw new Error(errorData.message || `HTTP error! status: ${response.status}`);
            }
            
            return await response.json();
        } catch (error) {
            console.error('API request failed:', error);
            throw error;
        }
    }

    // Expense endpoints
    async getExpenses(userId = null) {
        const queryParams = userId ? `?userId=${userId}` : '';
        return this.request(`/expense${queryParams}`);
    }

    async getExpense(id) {
        return this.request(`/expense/${id}`);
    }

    async createExpense(expenseData) {
        return this.request('/expense', {
            method: 'POST',
            body: JSON.stringify(expenseData)
        });
    }

    async updateExpense(id, expenseData) {
        return this.request(`/expense/${id}`, {
            method: 'PUT',
            body: JSON.stringify(expenseData)
        });
    }

    async deleteExpense(id) {
        return this.request(`/expense/${id}`, {
            method: 'DELETE'
        });
    }

    // Income endpoints
    async getIncomes(userId = null) {
        const queryParams = userId ? `?userId=${userId}` : '';
        return this.request(`/income${queryParams}`);
    }

    async getIncome(id) {
        return this.request(`/income/${id}`);
    }

    async createIncome(incomeData) {
        return this.request('/income', {
            method: 'POST',
            body: JSON.stringify(incomeData)
        });
    }

    async updateIncome(id, incomeData) {
        return this.request(`/income/${id}`, {
            method: 'PUT',
            body: JSON.stringify(incomeData)
        });
    }

    async deleteIncome(id) {
        return this.request(`/income/${id}`, {
            method: 'DELETE'
        });
    }

    // Category endpoints
    async getCategories() {
        return this.request('/category');
    }

    async getCategory(id) {
        return this.request(`/category/${id}`);
    }

    async createCategory(categoryData) {
        return this.request('/category', {
            method: 'POST',
            body: JSON.stringify(categoryData)
        });
    }

    async updateCategory(id, categoryData) {
        return this.request(`/category/${id}`, {
            method: 'PUT',
            body: JSON.stringify(categoryData)
        });
    }

    async deleteCategory(id) {
        return this.request(`/category/${id}`, {
            method: 'DELETE'
        });
    }

    // Budget endpoints
    async getBudgetGoals() {
        return this.request('/budget/goals');
    }

    async getBudgetGoal(id) {
        return this.request(`/budget/goals/${id}`);
    }

    async createBudgetGoal(goalData) {
        return this.request('/budget/goals', {
            method: 'POST',
            body: JSON.stringify(goalData)
        });
    }

    async deleteBudgetGoal(id) {
        return this.request(`/budget/goals/${id}`, {
            method: 'DELETE'
        });
    }

    async getBudgetProgress(categoryId = null) {
        const queryParams = categoryId ? `?categoryId=${categoryId}` : '';
        return this.request(`/budget/progress${queryParams}`);
    }

    // Report endpoints
    async getFinancialSummary(startDate = null, endDate = null) {
        let queryParams = '';
        if (startDate && endDate) {
            queryParams = `?startDate=${startDate}&endDate=${endDate}`;
        }
        return this.request(`/report/financial-summary${queryParams}`);
    }

    async getExpensesByCategory(startDate = null, endDate = null) {
        let queryParams = '';
        if (startDate && endDate) {
            queryParams = `?startDate=${startDate}&endDate=${endDate}`;
        }
        return this.request(`/report/expenses-by-category${queryParams}`);
    }

    async getTopExpenses(count = 10, startDate = null, endDate = null) {
        let queryParams = `?count=${count}`;
        if (startDate) queryParams += `&startDate=${startDate}`;
        if (endDate) queryParams += `&endDate=${endDate}`;
        return this.request(`/report/top-expenses${queryParams}`);
    }

    async getIncomeHistory(startDate = null, endDate = null) {
        let queryParams = '';
        if (startDate && endDate) {
            queryParams = `?startDate=${startDate}&endDate=${endDate}`;
        }
        return this.request(`/report/income-history${queryParams}`);
    }
}

// Create global API instance
const api = new BudgetAPI();

// jQuery-based API functions for backward compatibility
const APIService = {
    // Generic request function
    makeRequest: function(endpoint, options = {}) {
        const token = localStorage.getItem('authToken');
        
        return $.ajax({
            url: `/api${endpoint}`,
            type: options.method || 'GET',
            headers: {
                'Authorization': token ? `Bearer ${token}` : '',
                'Content-Type': 'application/json',
                ...options.headers
            },
            data: options.data ? JSON.stringify(options.data) : undefined,
            ...options,
            error: function(xhr, status, error) {
                if (xhr.status === 401) {
                    logout();
                }
                
                if (options.error) {
                    options.error(xhr, status, error);
                } else {
                    console.error('API request failed:', error);
                }
            }
        });
    },

    // Expenses
    getExpenses: function(callback, errorCallback) {
        return this.makeRequest('/expense', {
            success: callback,
            error: errorCallback
        });
    },

    createExpense: function(data, callback, errorCallback) {
        return this.makeRequest('/expense', {
            method: 'POST',
            data: data,
            success: callback,
            error: errorCallback
        });
    },

    // Incomes
    getIncomes: function(callback, errorCallback) {
        return this.makeRequest('/income', {
            success: callback,
            error: errorCallback
        });
    },

    createIncome: function(data, callback, errorCallback) {
        return this.makeRequest('/income', {
            method: 'POST',
            data: data,
            success: callback,
            error: errorCallback
        });
    },

    // Categories
    getCategories: function(callback, errorCallback) {
        return this.makeRequest('/category', {
            success: callback,
            error: errorCallback
        });
    },

    createCategory: function(data, callback, errorCallback) {
        return this.makeRequest('/category', {
            method: 'POST',
            data: data,
            success: callback,
            error: errorCallback
        });
    },

    // Reports
    getFinancialSummary: function(startDate, endDate, callback, errorCallback) {
        let endpoint = '/report/financial-summary';
        if (startDate && endDate) {
            endpoint += `?startDate=${startDate}&endDate=${endDate}`;
        }
        
        return this.makeRequest(endpoint, {
            success: callback,
            error: errorCallback
        });
    },

    getExpensesByCategory: function(startDate, endDate, callback, errorCallback) {
        let endpoint = '/report/expenses-by-category';
        if (startDate && endDate) {
            endpoint += `?startDate=${startDate}&endDate=${endDate}`;
        }
        
        return this.makeRequest(endpoint, {
            success: callback,
            error: errorCallback
        });
    }
};

// Export for use in other files
window.BudgetAPI = BudgetAPI;
window.api = api;
window.APIService = APIService;