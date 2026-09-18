using BigCommerceApi.Domain.Services.Auth;

public class NavbarService : INavbarService
{
    public string GetNavbarHtml(string activePage)
    {
        return $@"
<nav class='navbar'>
    <a href='#' class='navbar-brand'>Brand</a>
    <ul class='navbar-nav'>
        <li class='nav-item'><a href='/auth/home' class='nav-link {(activePage == "Home" ? "active" : "")}'>Home</a></li>
        <li class='nav-item'><a href='/auth/about' class='nav-link {(activePage == "About" ? "active" : "")}'>About</a></li>
        <li class='nav-item'><a href='/auth/orderDetails' class='nav-link {(activePage == "OrderDetails" ? "active" : "")}'>Order Details</a></li>
    </ul>
</nav>";
    }
}

