// Authentication and user management functions

let currentUser = null;

$(document).ready(function() {
    checkAuthentication();
});

function checkAuthentication() {
    const token = localStorage.getItem('authToken');
    const userData = localStorage.getItem('userData');
    
    if (token && userData) {
        currentUser = JSON.parse(userData);
        showAuthenticatedUI();
    } else {
        showUnauthenticatedUI();
    }
}

function showAuthenticatedUI() {
    $('#auth-buttons').hide();
    $('#user-menu').show();
    $('#sidebar').removeClass('d-none');
    $('#main-content').removeClass('col-md-10').addClass('col-md-10');
    
    if (currentUser) {
        $('#user-name').text(currentUser.name || currentUser.email);
        $('#welcome-user').text(currentUser.name || 'User');
    }
}

function showUnauthenticatedUI() {
    $('#auth-buttons').show();
    $('#user-menu').hide();
    $('#sidebar').addClass('d-none');
    $('#main-content').removeClass('col-md-10').addClass('col-12');
}

function login(email, password) {
    showLoading();
    
    return $.ajax({
        url: '/api/auth/login',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            email: email,
            password: password
        }),
        success: function(response) {
            localStorage.setItem('authToken', response.token);
            localStorage.setItem('userData', JSON.stringify({
                userId: response.userId,
                email: response.email,
                name: response.name
            }));
            
            currentUser = {
                userId: response.userId,
                email: response.email,
                name: response.name
            };
            
            showAuthenticatedUI();
            showSuccess('Login successful!');
            navigateTo('dashboard');
        },
        error: function(xhr) {
            let errorMessage = 'Login failed. Please try again.';
            
            if (xhr.status === 401) {
                errorMessage = 'Invalid email or password.';
            } else if (xhr.responseJSON && xhr.responseJSON.message) {
                errorMessage = xhr.responseJSON.message;
            }
            
            showError(errorMessage);
        },
        complete: function() {
            hideLoading();
        }
    });
}

function register(userData) {
    showLoading();
    
    return $.ajax({
        url: '/api/auth/register',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(userData),
        success: function(response) {
            localStorage.setItem('authToken', response.token);
            localStorage.setItem('userData', JSON.stringify({
                userId: response.userId,
                email: response.email,
                name: response.name
            }));
            
            currentUser = {
                userId: response.userId,
                email: response.email,
                name: response.name
            };
            
            showAuthenticatedUI();
            showSuccess('Registration successful! Welcome to Budget Manager.');
            navigateTo('dashboard');
        },
        error: function(xhr) {
            let errorMessage = 'Registration failed. Please try again.';
            
            if (xhr.responseJSON && xhr.responseJSON.errors) {
                const errors = xhr.responseJSON.errors;
                errorMessage = Object.values(errors).flat().join(' ');
            } else if (xhr.responseJSON && xhr.responseJSON.message) {
                errorMessage = xhr.responseJSON.message;
            }
            
            showError(errorMessage);
        },
        complete: function() {
            hideLoading();
        }
    });
}

function logout() {
    localStorage.removeItem('authToken');
    localStorage.removeItem('userData');
    currentUser = null;
    
    showUnauthenticatedUI();
    showInfo('You have been logged out successfully.');
    navigateTo('login');
}

function refreshToken() {
    const token = localStorage.getItem('authToken');
    
    if (!token) {
        logout();
        return;
    }
    
    return $.ajax({
        url: '/api/auth/refresh',
        type: 'POST',
        headers: {
            'Authorization': 'Bearer ' + token
        },
        success: function(response) {
            localStorage.setItem('authToken', response.token);
            localStorage.setItem('userData', JSON.stringify({
                userId: response.userId,
                email: response.email,
                name: response.name
            }));
        },
        error: function(xhr) {
            if (xhr.status === 401) {
                logout();
            }
        }
    });
}

// Auto-refresh token every 30 minutes
setInterval(function() {
    if (localStorage.getItem('authToken')) {
        refreshToken();
    }
}, 30 * 60 * 1000);

// Check token on page focus
$(window).on('focus', function() {
    if (localStorage.getItem('authToken')) {
        refreshToken();
    }
});

function getCurrentUser() {
    return currentUser;
}

function isAuthenticated() {
    return currentUser !== null && localStorage.getItem('authToken') !== null;
}