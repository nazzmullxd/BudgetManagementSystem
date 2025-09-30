# Budget Management System

A comprehensive, modern web-based budget management application built with ASP.NET Core 10.0, featuring real-time financial tracking, interactive dashboards, and secure JWT authentication.

## 🌟 Features

### 💰 Financial Management
- **Expense Tracking**: Complete CRUD operations for expense management
- **Income Management**: Track multiple income sources with detailed categorization
- **Budget Goals**: Set and monitor financial goals with progress tracking
- **Categories**: Flexible expense and income categorization system
- **Multi-Currency Support**: Handle transactions in different currencies

### 📊 Analytics & Reporting
- **Interactive Dashboard**: Real-time financial overview with charts and statistics
- **Visual Reports**: Dynamic charts using Chart.js for expense/income analysis
- **Date Range Filtering**: Analyze financial data across custom time periods
- **Category Breakdown**: Detailed spending analysis by category
- **Financial Trends**: Track spending patterns and income growth

### 🔐 Security & Authentication
- **JWT Authentication**: Secure token-based authentication system
- **User Registration**: Comprehensive user signup with validation
- **Password Security**: Secure password hashing and validation
- **Session Management**: Proper token refresh and logout handling
- **Role-based Access**: Secure API endpoints with authorization

### 🎨 Modern User Interface
- **Responsive Design**: Mobile-first approach with Bootstrap 5.3.2
- **SPA Experience**: Single-page application feel with dynamic navigation
- **Interactive Charts**: Real-time data visualization
- **Modern UI Components**: Clean, professional interface design
- **Dark/Light Mode**: Adaptive styling for better user experience

### 🚀 Technical Features
- **RESTful APIs**: Complete REST API with Swagger documentation
- **Real-time Updates**: Dynamic content loading without page refresh
- **Data Validation**: Client and server-side validation
- **Error Handling**: Comprehensive error management and user feedback
- **Audit Logging**: Track user actions and system changes

## 🏗️ Architecture

### Technology Stack
- **Backend**: ASP.NET Core 10.0
- **Database**: Entity Framework Core 8.0
- **Frontend**: Bootstrap 5.3.2, jQuery 3.7.1, Chart.js 4.4.0
- **Authentication**: JWT Bearer Tokens
- **API Documentation**: Swagger/OpenAPI
- **Styling**: CSS Grid, Flexbox, Bootstrap Icons

### Project Structure
```
BudgetManagementSystem/
├── Database/                    # Data layer and Entity Framework
│   ├── Context/                # Database context configuration
│   ├── Model/                  # Entity models and data structures
│   ├── Repositories/           # Repository pattern implementation
│   └── Migrations/             # Database migrations
├── Business/                   # Business logic layer
│   └── Services/               # Business services and logic
├── WEB/                       # Web application layer
│   ├── Controllers/           # API controllers
│   ├── Models/                # View models and DTOs
│   ├── Pages/                 # Razor pages
│   ├── wwwroot/               # Static assets (CSS, JS, images)
│   └── Views/                 # MVC views
└── API/                       # Additional API services
```

### Database Schema
The system uses a comprehensive database schema with the following key entities:
- **Users**: User accounts and authentication
- **TrackExpense**: Expense transactions and details
- **TrackIncome**: Income records and sources
- **ExpenseCategory**: Expense categorization
- **BudgetGoal**: Financial goals and targets
- **Currency**: Multi-currency support
- **AuditLog**: System activity tracking

## 🚀 Getting Started

### Prerequisites
- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/sql-server) or SQL Server LocalDB
- [Node.js](https://nodejs.org/) (for development tools)
- [Git](https://git-scm.com/)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/BudgetManagementSystem.git
   cd BudgetManagementSystem
   ```

2. **Configure Database Connection**
   
   Update the connection string in `WEB/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BudgetManagementDB;Trusted_Connection=true;MultipleActiveResultSets=true"
     }
   }
   ```

3. **Configure JWT Settings**
   
   Update JWT configuration in `WEB/appsettings.json`:
   ```json
   {
     "Jwt": {
       "Key": "your-super-secure-secret-key-here",
       "Issuer": "BudgetManagementSystem",
       "Audience": "BudgetManagementUsers",
       "ExpireMinutes": 60
     }
   }
   ```

4. **Install Dependencies**
   ```bash
   dotnet restore
   ```

5. **Run Database Migrations**
   ```bash
   dotnet ef database update --project Database --startup-project WEB
   ```

6. **Build the Solution**
   ```bash
   dotnet build
   ```

7. **Run the Application**
   ```bash
   dotnet run --project WEB
   ```

8. **Access the Application**
   - HTTPS: https://localhost:7282
   - HTTP: http://localhost:5175
   - Swagger API Documentation: https://localhost:7282/swagger

## 📱 Usage

### Getting Started
1. **Registration**: Create a new account using the registration page
2. **Login**: Sign in with your credentials to access the dashboard
3. **Dashboard**: View your financial overview and recent transactions
4. **Add Expenses**: Record your daily expenses with categories
5. **Track Income**: Log income from various sources
6. **Set Goals**: Create budget goals and monitor progress
7. **View Reports**: Analyze your financial data with interactive charts

### API Usage

The application provides a comprehensive REST API. Here are some key endpoints:

#### Authentication
```http
POST /api/auth/login
POST /api/auth/register
```

#### Expenses
```http
GET    /api/expense           # Get all expenses
POST   /api/expense           # Create new expense
PUT    /api/expense/{id}      # Update expense
DELETE /api/expense/{id}      # Delete expense
```

#### Income
```http
GET    /api/income            # Get all income
POST   /api/income            # Create new income
PUT    /api/income/{id}       # Update income
DELETE /api/income/{id}       # Delete income
```

#### Budget Goals
```http
GET    /api/budget/goals      # Get budget goals
POST   /api/budget/goals      # Create new goal
DELETE /api/budget/goals/{id} # Delete goal
```

#### Reports
```http
GET /api/report/summary       # Financial summary
GET /api/report/expenses      # Expense analysis
GET /api/report/income        # Income analysis
```

### Example API Request
```bash
# Login to get JWT token
curl -X POST https://localhost:7282/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"user@example.com","password":"password"}'

# Create expense with JWT token
curl -X POST https://localhost:7282/api/expense \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -d '{
    "itemName": "Groceries",
    "itemPrice": 75.50,
    "quantity": 1,
    "expenseCategoryId": "category-id",
    "transactionDate": "2025-09-30T00:00:00Z",
    "currencyId": "currency-id"
  }'
```

## 🛠️ Development

### Setting up Development Environment

1. **Install Development Tools**
   ```bash
   dotnet tool install --global dotnet-ef
   dotnet tool install --global dotnet-aspnet-codegenerator
   ```

2. **Database Migrations**
   ```bash
   # Create new migration
   dotnet ef migrations add MigrationName --project Database --startup-project WEB
   
   # Update database
   dotnet ef database update --project Database --startup-project WEB
   ```

3. **Running Tests**
   ```bash
   dotnet test
   ```

### Code Structure Guidelines

- **Controllers**: Handle HTTP requests and responses
- **Services**: Contain business logic and data processing
- **Repositories**: Handle data access and database operations
- **Models**: Define data structures and validation rules
- **DTOs**: Data Transfer Objects for API communication

### Adding New Features

1. **Database Changes**: Create migrations for schema updates
2. **Models**: Add or update entity models in the Database project
3. **Repositories**: Implement data access methods
4. **Services**: Add business logic in the Business project
5. **Controllers**: Create API endpoints in the WEB project
6. **Frontend**: Update UI components and JavaScript

## 🔧 Configuration

### Environment Variables
```bash
# Development
ASPNETCORE_ENVIRONMENT=Development

# Production
ASPNETCORE_ENVIRONMENT=Production
```

### Application Settings

Key configuration options in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "your-database-connection-string"
  },
  "Jwt": {
    "Key": "your-secret-key",
    "Issuer": "BudgetManagementSystem",
    "Audience": "BudgetManagementUsers",
    "ExpireMinutes": 60
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

## 📊 Database Schema

### Core Entities

**Users Table**
- UserId (Primary Key)
- FirstName, LastName
- Email (Unique)
- PasswordHash
- PhoneNumber
- CreatedAt, UpdatedAt

**TrackExpense Table**
- TrackExpenseId (Primary Key)
- ItemName, ItemPrice, Quantity
- TransactionDate
- UserId (Foreign Key)
- ExpenseCategoryId (Foreign Key)
- CurrencyId (Foreign Key)

**TrackIncome Table**
- IncomeId (Primary Key)
- IncomeSource, IncomeType
- IncomeAmount, IncomeTax
- IncomeDate, Frequency
- UserId (Foreign Key)
- CurrencyId (Foreign Key)

**BudgetGoal Table**
- BudgetGoalId (Primary Key)
- GoalName, TargetAmount
- TargetDate, CurrentAmount
- UserId (Foreign Key)
- ExpenseCategoryId (Foreign Key)

## 🔒 Security

### Authentication & Authorization
- **JWT Tokens**: Secure stateless authentication
- **Password Hashing**: BCrypt for secure password storage
- **HTTPS**: SSL/TLS encryption for all communications
- **CORS**: Configured for secure cross-origin requests

### Data Protection
- **Input Validation**: Server and client-side validation
- **SQL Injection Prevention**: Entity Framework parameterized queries
- **XSS Protection**: Proper output encoding
- **CSRF Protection**: Anti-forgery tokens

### Best Practices
- Regular security updates
- Secure configuration management
- Proper error handling without information leakage
- Audit logging for security events

## 📈 Performance

### Optimization Features
- **Entity Framework Optimization**: Efficient queries and caching
- **Async Operations**: Non-blocking database operations
- **Static Asset Optimization**: Minified CSS and JavaScript
- **Response Compression**: Gzip compression enabled
- **Database Indexing**: Optimized database indexes

### Monitoring
- **Application Insights**: Performance monitoring
- **Structured Logging**: Comprehensive logging with Serilog
- **Health Checks**: Application and database health monitoring

## 🚀 Deployment

### Production Deployment

1. **Build for Production**
   ```bash
   dotnet publish -c Release -o ./publish
   ```

2. **Environment Configuration**
   - Update connection strings
   - Configure JWT secrets
   - Set up HTTPS certificates
   - Configure logging levels

3. **Database Setup**
   ```bash
   dotnet ef database update --connection "ProductionConnectionString"
   ```

### Docker Deployment
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY ./publish .
EXPOSE 80 443
ENTRYPOINT ["dotnet", "WEB.dll"]
```

### Cloud Deployment Options
- **Azure App Service**: Easy deployment with CI/CD
- **AWS Elastic Beanstalk**: Scalable web application hosting
- **Google Cloud Platform**: Container-based deployment
- **Docker Containers**: Containerized deployment

## 🧪 Testing

### Test Coverage
- Unit Tests for business logic
- Integration tests for API endpoints
- Database tests for data access
- Frontend tests for UI components

### Running Tests
```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test Tests/Business.Tests

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

## 🤝 Contributing

### Development Workflow
1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Code Standards
- Follow C# coding conventions
- Use meaningful variable and method names
- Write comprehensive unit tests
- Document public APIs
- Follow SOLID principles

### Pull Request Guidelines
- Provide clear description of changes
- Include tests for new features
- Update documentation as needed
- Ensure all tests pass
- Follow code review feedback

## 📝 Changelog

### Version 2.0.0 (Current)
- ✅ Complete UI modernization with Bootstrap 5
- ✅ JWT authentication implementation
- ✅ REST API with Swagger documentation
- ✅ Interactive dashboard with Chart.js
- ✅ SPA-style navigation and user experience
- ✅ Comprehensive expense and income management
- ✅ Budget goal tracking and progress monitoring
- ✅ Multi-currency support
- ✅ Responsive mobile-first design
- ✅ Real-time data visualization

### Version 1.0.0 (Legacy)
- Basic expense tracking
- Simple user management
- Basic reporting features

## 🐛 Known Issues

- None currently reported

## 📞 Support

### Getting Help
- **Documentation**: Check this README and inline code comments
- **Issues**: Report bugs and feature requests on GitHub Issues
- **Discussions**: Join community discussions on GitHub Discussions

### Contact
- **Email**: [your-email@example.com]
- **GitHub**: [Your GitHub Profile]
- **LinkedIn**: [Your LinkedIn Profile]

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- **ASP.NET Core Team**: For the excellent web framework
- **Entity Framework Team**: For the robust ORM
- **Bootstrap Team**: For the responsive UI framework
- **Chart.js Team**: For the beautiful charting library
- **Community Contributors**: For feedback and contributions

---

## 🎯 Future Roadmap

### Planned Features
- [ ] Mobile app development (React Native/Flutter)
- [ ] Advanced analytics and AI insights
- [ ] Bank account integration
- [ ] Receipt scanning with OCR
- [ ] Multi-language support
- [ ] Dark theme implementation
- [ ] Export/import functionality
- [ ] Notification system
- [ ] Advanced reporting with PDF generation
- [ ] Social features and family budget sharing

### Performance Improvements
- [ ] Redis caching implementation
- [ ] Database query optimization
- [ ] CDN integration for static assets
- [ ] Progressive Web App (PWA) features
- [ ] Offline functionality

---

**Made with ❤️ using ASP.NET Core and modern web technologies**

*Last updated: September 30, 2025*