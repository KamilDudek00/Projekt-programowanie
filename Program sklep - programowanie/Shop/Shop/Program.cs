using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Shop.Helpers;
using Shop.Models;
using Shop.Repositories;
using System.Drawing;
using System.Reflection.Metadata;

int currentIndex = 0;
var inProgress = true;
var clientMenuInProgress = false;
var sellerMenuInProgress = false;
var clientsListInProgress = false;
var magazineInProgress = false;
var ordersInProgress = false;
var userBasketInProgress = false;
var productsListInProgress = false;
var productViewMenuInProgress = false;
var sizeMenuInProgress = false;
var basketMenuInProgress = false;
var basketMenuRemoveInProgress = false;
var myAccountInProgress = false;
var orderMenuInProgress = false;
var sellerLoginMenuInProgress = false;


var clientPassword = string.Empty;
var isLoggedIn = false;
int currentProductIndex = 0;
var loggedUser = new User();

Console.CursorVisible = false;
Dictionary<string, string> collection = new Dictionary<string, string>
{
    { "tshirt", Clothes.Tshirt },
    { "skirt", Clothes.Skirt },
    { "pants", Clothes.Pants },
    { "shoes", Clothes.Shoes },
    { "glasses", Clothes.Glasses },
    { "gloves", Clothes.Gloves },
    { "blouse", Clothes.Blouse },
    { "dress", Clothes.Dress }
};

var mainMenu = new List<string>() { "Klient", "Sprzedawca" };
var clientMenu = new List<string>() { "Zaloguj", "Zarejestruj", "Wyjście" };
var userMenu = new List<string>() {
                "Koszyk",
                "Moje Konto",
                "Lista produktów",
                "Wyloguj"
            };

var sellerMenu = new List<string>() {
                "Lista klientów",
                "Stan Magazynu",
                "Realizacja zamówień",
                "Wyjście"
            };

var magazineMenu = new List<string>() {
                "Dodaj produkt",
                "Usuń produkt o podanym numerze seryjnym",
                "Edytuj produkt o podanym numerze seryjnym",
                "Sortuj produkty po nazwie",
                "Generuj raport PDF",
                "Wyjście"
            };

var ordersMenu = new List<string>() {
                "Usuń zamówienie z historii zamówień",
                "Wyślij zamówienie",
                "Wyjście"
            };

var productViewOptions = new List<string>
            {
                "Mój koszyk",
                "Rozmiar",
                "Ilość",
                "Dodaj do koszyka",
                "Wyjście"
            };

var basketOptionsMenu = new List<string>
            {
                "Usuń z koszyka",
                "Zatwierdź zakupy",
                "Wyjście"
            };

using (var dbContext = new ApplicationDbContext())
{
    var userRepository = new UserRepository(dbContext);
    var productRepository = new ProductRepository(dbContext);
    var orderRepository = new OrderRepository(dbContext);
    var roleRepository = new RoleRepository(dbContext);

    while (inProgress)
    {
        Console.Clear();
        Console.WriteLine(Symbols.Shop, Color.FromArgb(153, 255, 255));
        string selectedMenuItem = drawMainMenu(mainMenu, 20, 10);
        if (selectedMenuItem == "Klient")
        {
            clientMenuInProgress = true;
            while (clientMenuInProgress)
            {
                Console.SetCursorPosition(20, 1);
                Console.WriteLine("Nie masz jeszcze konta? Zarejestruj się!");
                kreska(70);
                Console.Write(Symbols.Book, Color.FromArgb(153, 255, 255));

                string selectedMenuClient = drawMenu(clientMenu, 30, 6);

                if (selectedMenuClient == "Zaloguj")
                {
                    Console.Clear();
                    Console.SetCursorPosition(0, 1);
                    Console.WriteLine("Login: ");
                    var login = Console.ReadLine();
                    Console.SetCursorPosition(0, 4);
                    Console.WriteLine("Hasło: ");
                    typePassword();

                    var user = userRepository.GetAllEntities().Where(x => x.Email == login && x.Role.Name == RoleType.Client.ToString()).FirstOrDefault();
                    if (user != null)
                    {
                        if (user.Password == clientPassword)
                        {
                            viewMessage("Pomyślnie zalogowano.", ConsoleColor.Green);
                            isLoggedIn = true;
                            loggedUser = user;
                            clientPassword = string.Empty;
                        }
                        else
                        {
                            viewMessage("Dane logowania nie są prawidłowe.", ConsoleColor.Red);
                            clientPassword = string.Empty;
                        }
                    }
                    else
                    {
                        viewMessage("Dane logowania nie są prawidłowe.", ConsoleColor.Red);
                        clientPassword = string.Empty;
                    }

                    while (isLoggedIn)
                    {
                        Console.Clear();
                        Console.WriteLine("Witaj " + loggedUser.FirstName + " " + loggedUser.LastName +"!\n");
                        var selectedItem = drawMenu(userMenu, 30, 1);
                        if (selectedItem == "Koszyk")
                        {
                            userBasketInProgress = true;
                            var products = productRepository.GetAllProducts().ToList();
                            while (userBasketInProgress)
                            {
                                viewBasket(products, orderRepository, productRepository);
                            }
                        }
                        else if (selectedItem == "Lista produktów")
                        {
                            bool productsAscending = false;
                            string searchPhase = string.Empty;
                            Console.Clear();
                            var products = new List<Product>();

                            var productsListMenu = new List<string>();

                            products = productRepository.GetAllProducts().ToList();
                            Console.WriteLine(products.Count());


                            for (int i = 0; i < products.Count(); i++)
                            {
                                if (collection.ContainsKey(products[i].Image.ToLower()))
                                {
                                    productsListMenu.Add(collection[products[i].Image.ToLower()] + products[i].Name + "   Cena: " + products[i].Price + " zł");
                                }
                                else
                                {
                                    productsListMenu.Add("Brak obrazka " + products[i].Name + "   Cena: " + products[i].Price + " zł");
                                }                                   
                            }

                            productsListMenu.Add("Szukaj po nazwie");
                            productsListMenu.Add("Sortuj po nazwie");
                            productsListMenu.Add("Wyjście");
                            productsListInProgress = true;
                            while (productsListInProgress)
                            {
                                Console.Clear();
                                var productSelectedItem = drawMenuProducts(productsListMenu);
                                if (productSelectedItem == "Wyjście")
                                {
                                    productsListInProgress= false;
                                }
                                else if (productSelectedItem == "Szukaj po nazwie")
                                {
                                    Console.Clear();
                                    Console.SetCursorPosition(0, 0); kreska(15); Console.SetCursorPosition(16, 0); Console.WriteLine("Wyszukiwanie "); kreska(15);
                                    Console.SetCursorPosition(0, 1); kreska(15); Console.SetCursorPosition(16, 1); Console.Write("Wyszukaj produkt:"); kreska(15);
                                    productsListMenu = new List<string>();
                                    searchPhase = Console.ReadLine();
                                    products = productRepository.GetAllProducts().Where(x => x.Name.ToLower().Contains(searchPhase.ToLower())).ToList();
                                    for (int i = 0; i < products.Count(); i++)
                                    {
                                        if (collection.ContainsKey(products[i].Image.ToLower()))
                                        {
                                            productsListMenu.Add(collection[products[i].Image.ToLower()] + products[i].Name);
                                        }
                                        else
                                        {
                                            productsListMenu.Add("Brak obrazka " + products[i].Name);
                                        }
                                        
                                    }

                                    productsListMenu.Add("Szukaj po nazwie");
                                    productsListMenu.Add("Sortuj po nazwie");
                                    productsListMenu.Add("Wyjście");

                                }
                                else if (productSelectedItem == "Sortuj po nazwie")
                                {
                                    if (productsAscending)
                                    {
                                        products = productRepository.GetAllProducts().OrderBy(x => x.Name).ToList();
                                        productsAscending = false;
                                    }
                                    else
                                    {
                                        products = productRepository.GetAllProducts().OrderByDescending(x => x.Name).ToList();
                                        productsAscending = true;
                                    }

                                    productsListMenu.Clear();
                                    for (int i = 0; i < products.Count(); i++)
                                    {
                                        if (collection.ContainsKey(products[i].Image.ToLower()))
                                        {
                                            productsListMenu.Add(collection[products[i].Image.ToLower()] + products[i].Name + "   Cena: " + products[i].Price + " zł");
                                        }
                                        else
                                        {
                                            productsListMenu.Add("Brak obrazka " + products[i].Name + "   Cena: " + products[i].Price + " zł");
                                        }
                                    }
                                    productsListMenu.Add("Szukaj po nazwie");
                                    productsListMenu.Add("Sortuj po nazwie");
                                    productsListMenu.Add("Wyjście");
                                }
                                else if (!string.IsNullOrWhiteSpace(productSelectedItem))
                                {
                                    productViewMenuInProgress = true;
                                    string chosenSize = string.Empty;
                                    int chosenAmount = 0;

                                    while (productViewMenuInProgress)
                                    {
                                        Console.Clear();
                                        Console.SetCursorPosition(0, 1);
                                        try
                                        {
                                            Console.WriteLine(collection[products[currentProductIndex].Image.ToLower()]);
                                        }
                                        catch
                                        {
                                            Console.WriteLine("Brak obrazka");
                                        }
                                        Console.WriteLine(products[currentProductIndex].Name);
                                        Console.WriteLine("\nWybrany rozmiar: " + chosenSize);
                                        Console.WriteLine("\nWybrana ilość: " + chosenAmount);
                                        string optionBasket = drawMenu(productViewOptions, 30, 1);
                                        if (optionBasket == "Rozmiar")
                                        {
                                            var availableSizes = products[currentProductIndex].Size.ToLower().Split(',');

                                            Console.WriteLine("\nWybierz rozmiar:");
                                            foreach (var size in availableSizes)
                                            {
                                                Console.Write(size + " ");
                                            }
                                            Console.Write(":");
                                            chosenSize = Console.ReadLine();
                                            if (!availableSizes.Contains(chosenSize.ToLower()))
                                            {
                                                Console.Clear();
                                                viewMessage("Podano niewłaściwy rozmiar.", ConsoleColor.Red);
                                                chosenSize = string.Empty;
                                            }

                                        }
                                        else if (optionBasket == "Ilość")
                                        {
                                            Console.WriteLine("\nPodaj ilosc:");
                                            Console.Write(products[currentProductIndex].Amount + " / ");


                                            if (int.TryParse(Console.ReadLine(), out chosenAmount))
                                            {
                                                if (chosenAmount > products[currentProductIndex].Amount)
                                                {
                                                    chosenAmount = 0;
                                                    Console.Clear();
                                                    viewMessage("Wybrana ilość produktu jest niedostępna.", ConsoleColor.Red);
                                                }
                                            }
                                            else
                                            {
                                                viewMessage("Wybrana ilość produktu jest niepoprawna.", ConsoleColor.Red);
                                            }
                                        }
                                        else if (optionBasket == "Dodaj do koszyka")
                                        {
                                            var productToBasket = new Product(products[currentProductIndex]);
                                            if (chosenAmount <= 0)
                                            {
                                                viewMessage("Ilość produku musi być większa niż 0.", ConsoleColor.Red);
                                            }
                                            else if (string.IsNullOrWhiteSpace(chosenSize))
                                            {
                                                viewMessage("Rozmiar produku musi zostać wybrany", ConsoleColor.Red);
                                            }
                                            else
                                            {
                                                productToBasket.Amount = chosenAmount;
                                                productToBasket.Size = chosenSize;
                                                Basket.AddProduct(productToBasket);
                                            }

                                        }
                                        else if (optionBasket == "Mój koszyk")
                                        {
                                            basketMenuInProgress = true;
                                            while (basketMenuInProgress)
                                            {
                                                viewBasket(products, orderRepository, productRepository);
                                            }
                                        }
                                        else if (optionBasket == "Wyjście")
                                        {
                                            productViewMenuInProgress = false;
                                        }
                                    }
                                }
                            }
                        }
                        else if (selectedItem == "Moje Konto")
                        {
                            myAccountInProgress = true;
                            while (myAccountInProgress)
                            {
                                var labels = new List<string>();
                                labels.Add("Moje dane");
                                labels.Add("Imie: ");
                                labels.Add("Nazwisko: ");
                                labels.Add("Email: ");
                                labels.Add("Adres: ");
                                labels.Add("Hasło: ");

                                var clientData = new List<string>();
                                clientData.Add(loggedUser.FirstName);
                                clientData.Add(loggedUser.LastName);
                                clientData.Add(loggedUser.Email);
                                clientData.Add(loggedUser.City);
                                clientData.Add(loggedUser.Password);
                                justShow(labels, 50, 2, 0);
                                justShow(clientData, 61, 4, 1);
                                Console.SetCursorPosition(1, 1);

                                var clientAccountMenu = new List<string>();
                                var userOrders = orderRepository.GetUserOrders(loggedUser.Id);
                                for (int i = 0; i < userOrders.Count(); i++)
                                {
                                    clientAccountMenu.Add("Zamowienie " + (i + 1).ToString());
                                }
                                clientAccountMenu.Add("Wyjście");
                                Console.Write("Moja historia zamówień      Liczba zamówień: " + (clientAccountMenu.Count-1));
                                string selectedMenuClientAccount = drawMenu(clientAccountMenu, 30, 3);
                                if (selectedMenuClientAccount == "Wyjście")
                                {
                                    myAccountInProgress = false;
                                }
                                else if (selectedMenuClientAccount == null) { }
                                else
                                {
                                    var number = selectedMenuClientAccount.Split(" ")[1];
                                    if (int.TryParse(number, out var orderNumber))
                                    {
                                        orderNumber = orderNumber - 1;
                                        var chosenOrder = userOrders[orderNumber];
                                        orderMenuInProgress = true;
                                        while (orderMenuInProgress)
                                        {
                                            Console.Clear();
                                            foreach (var order in chosenOrder.ProductOrders)
                                            {
                                                if (collection.ContainsKey(order.Product.Image.ToLower()))
                                                {
                                                    Console.Write(collection[order.Product.Image.ToLower()]);
                                                }

                                                Console.WriteLine("   "+ order.Product.Name + " " + order.Product.Price + "zł " + order.ProductAmount + " sztuk\n");
                                            }
                                            Console.WriteLine("\nWciśnij Escape, aby zamknąć to okno");
                                            ConsoleKeyInfo enter = Console.ReadKey();

                                            if (enter.Key == ConsoleKey.Escape)
                                            {
                                                orderMenuInProgress = false;
                                                Console.Clear();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if(selectedItem == "Wyloguj")
                        {
                            loggedUser = null;
                            clientMenuInProgress = false;
                            isLoggedIn = false;
                        }
                    }

                }
                else if (selectedMenuClient == "Zarejestruj")
                {
                    Console.Clear();

                    Console.SetCursorPosition(0, 1); Console.Write("Imie:"); string name = Console.ReadLine();
                    Console.SetCursorPosition(0, 3); Console.Write("Nazwisko:"); string surname = Console.ReadLine();
                    Console.SetCursorPosition(0, 5); Console.Write("Email:"); string email = Console.ReadLine();
                    Console.SetCursorPosition(0, 7); Console.Write("Miasto:"); string address = Console.ReadLine();
                    Console.SetCursorPosition(0, 9); Console.Write("Hasło:"); string newpassword = Console.ReadLine();
                    Console.SetCursorPosition(0, 11); Console.Write("Powtórz hasło:"); string rnewpassword = Console.ReadLine();

                    if (newpassword != rnewpassword)
                    {
                        viewMessage("Hasła nie zgadzają się. Wciśnij dowolny przycisk.", ConsoleColor.Red);
                    }
                    else
                    {
                        var user = new User
                        {
                            FirstName = name,
                            LastName = surname,
                            Email = email,
                            City = address,
                            Password = newpassword,
                            Role = roleRepository.GetRole(RoleType.Client)
                        };

                        userRepository.AddEntity(user);
                        viewMessage("Pomyślnie utworzono konto.", ConsoleColor.Green);
                    }
                }
                else if (selectedMenuClient == "Wyjście")
                {
                    Console.Clear();
                    clientMenuInProgress = false;
                    Environment.Exit(0);
                }
            }
        }
        else if (selectedMenuItem == "Sprzedawca")
        {
            sellerLoginMenuInProgress = true;
            while (sellerLoginMenuInProgress)
            {
                Console.SetCursorPosition(20, 1);
                Console.WriteLine("Nie masz jeszcze konta? Zarejestruj się!");
                kreska(70);
                Console.Write(Symbols.Book, Color.FromArgb(153, 255, 255));

                string selectedMenuLogin = drawMenu(clientMenu, 30, 6);

                if (selectedMenuLogin == "Zaloguj")
                {
                    Console.Clear();
                    Console.SetCursorPosition(0, 1);
                    Console.WriteLine("Login: ");
                    var login = Console.ReadLine();
                    Console.SetCursorPosition(0, 4);
                    Console.WriteLine("Hasło: ");
                    typePassword();

                    var user = userRepository.GetAllEntities().Where(x => x.Email == login && x.Role.Name == RoleType.Seller.ToString()).FirstOrDefault();
                    if (user != null)
                    {
                        if (user.Password == clientPassword)
                        {
                            viewMessage("Pomyślnie zalogowano.", ConsoleColor.Green);
                            sellerMenuInProgress = true;
                            loggedUser = user;
                            clientPassword = string.Empty;
                        }
                        else
                        {
                            viewMessage("Dane logowania nie są prawidłowe.", ConsoleColor.Red);
                            clientPassword = string.Empty;
                        }
                    }
                    else
                    {
                        viewMessage("Dane logowania nie są prawidłowe.", ConsoleColor.Red);
                        clientPassword = string.Empty;
                    }


                    while (sellerMenuInProgress)
                    {
                        Console.Clear();
                        kreska(100);
                        Console.SetCursorPosition(3, 4);
                        string selectedMenuSeller = drawMenu(sellerMenu, 30, 1);
                        if (selectedMenuSeller == "Lista klientów")
                        {
                            clientsListInProgress = true;
                            while (clientsListInProgress)
                            {
                                Console.Clear();
                                var clients = userRepository.GetAllEntities().Where(x => x.Role.Name == RoleType.Client.ToString());
                                if (!clients.Any())
                                {
                                    Console.SetCursorPosition(3, 3); kreska(40); Console.SetCursorPosition(8, 3); Console.WriteLine("Brak klientów w bazie klientów");
                                    Console.SetCursorPosition(3, 4); kreska(70);
                                    Console.SetCursorPosition(4, 6); Console.WriteLine("Escape - powrót do menu");
                                    ConsoleKeyInfo inf = Console.ReadKey();
                                    if (inf.Key == ConsoleKey.Escape)
                                    {
                                        clientsListInProgress = false;
                                    }
                                }
                                else
                                {
                                    Console.Clear(); Console.SetCursorPosition(3, 2); kreska(40); Console.SetCursorPosition(8, 2); Console.WriteLine("Lista Klientów ");
                                    kreska(100); Console.WriteLine("Delete - Usunięcie klienta po mailu");
                                    Console.WriteLine("Escape - powrót do menu"); kreska(100);
                                    Console.WriteLine("Imie    Nazwisko   Email   Adres Zamówienia"); kreska(100);
                                    foreach (var client in clients)
                                    {
                                        Console.WriteLine(client.FirstName + " " + client.LastName
                                             + "  " + client.Email + "   " + client.City + "  "
                                             + (client.Orders == null ? 0.ToString() : client.Orders.Count().ToString()), Color.FromArgb(179, 255, 179));
                                    }

                                    kreska(100);
                                    ConsoleKeyInfo info = Console.ReadKey();
                                    if (info.Key == ConsoleKey.Escape)
                                    {
                                        clientsListInProgress = false;
                                    }
                                    if (info.Key == ConsoleKey.Delete)
                                    {
                                        Console.Clear(); Console.SetCursorPosition(5, 5); kreska(40); Console.SetCursorPosition(10, 5); Console.WriteLine("Usuwanie klienta z listy ");
                                        Console.SetCursorPosition(5, 6); kreska(70);
                                        Console.SetCursorPosition(6, 10); Console.WriteLine("Podaj email klienta, którego chcesz usunąć");
                                        Console.SetCursorPosition(5, 15); kreska(70);
                                        Console.SetCursorPosition(5, 16);
                                        string email = Console.ReadLine();
                                        Console.SetCursorPosition(5, 17); Console.WriteLine("Wybrana pozycja z listy zostanie usunięta. Czy chcesz ją usunąć? ");
                                        Console.SetCursorPosition(7, 18); Console.WriteLine("Tak - Enter     |    Nie - Backspace");
                                        ConsoleKeyInfo inf = Console.ReadKey();
                                        if (inf.Key == ConsoleKey.Enter)
                                        {
                                            var userToBeRemoved = userRepository.GetUserByEmail(email);
                                            if (userToBeRemoved != null)
                                            {
                                                userRepository.RemoveUserById(userToBeRemoved.Id);
                                            }
                                            else
                                            {
                                                viewMessage("Użytkownik o podanym adresie e-mail nie istnieje.", ConsoleColor.Red);
                                            }
                                        }
                                        else if (inf.Key == ConsoleKey.Backspace)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else if (selectedMenuSeller == "Stan Magazynu")
                        {
                            magazineInProgress = true;
                            var products = productRepository.GetAllProducts();
                            var ascendingOrder = true;
                            while (magazineInProgress)
                            {

                                Console.Clear();
                                if (products.Count() <= 0)
                                {
                                    Console.SetCursorPosition(3, 0); kreska(40); Console.SetCursorPosition(30, 0); Console.WriteLine("Brak produktów w magazynie");
                                    Console.SetCursorPosition(56, 0); kreska(20);
                                }
                                else
                                {

                                    Console.SetCursorPosition(15, 17); Console.WriteLine("Stan Magazynu ");
                                    Console.SetCursorPosition(0, 18); kreska(100); Console.WriteLine("Nazwa    Numer Seryjny  Liczba Cena Rozmiar  Plec   Obrazek"); kreska(100);
                                    foreach (var product in products)
                                    {
                                        Console.WriteLine(product.Name + "     " + product.SerialNumber
                                            + " " + product.Amount + " " + product.Price
                                            + " " + product.Size + " " + product.Sex + " " + product.Image, Color.FromArgb(241, 180, 160));
                                    }
                                }

                                kreska(100);
                                string selectedMagazineStateItem = drawMenu(magazineMenu, 30, 3);
                                if (selectedMagazineStateItem == "Dodaj produkt")
                                {
                                    addProduct(productRepository);
                                    products = productRepository.GetAllProducts();
                                }
                                if (selectedMagazineStateItem == "Usuń produkt o podanym numerze seryjnym")
                                {
                                    Console.Clear(); Console.SetCursorPosition(0, 5); kreska(40);
                                    Console.SetCursorPosition(0, 0); kreska(70);
                                    Console.SetCursorPosition(0, 1); Console.WriteLine("Podaj numer seryjny produktu, który chcesz usunąć");
                                    Console.SetCursorPosition(0, 2); kreska(70);
                                    Console.SetCursorPosition(0, 4);
                                    Console.WriteLine("Nazwa    Numer Seryjny  Liczba Cena Rozmiar  Plec   Obrazek"); kreska(100);
                                    foreach (var product in products)
                                    {
                                        Console.WriteLine(product.Name + "     " + product.SerialNumber
                                            + " " + product.Amount + " " + product.Price
                                            + " " + product.Size + " " + product.Sex + " " + product.Image, Color.FromArgb(241, 180, 160));
                                    }
                                    string serialNumber = Console.ReadLine();
                                    if (!string.IsNullOrEmpty(serialNumber))
                                    {
                                        Console.SetCursorPosition(5, 17); Console.WriteLine("Wybrana pozycja z listy zostanie usunięta. Czy chcesz ją usunąć? ");
                                        Console.SetCursorPosition(7, 18); Console.WriteLine("Tak - Enter     |    Nie - Backspace");
                                        ConsoleKeyInfo inf = Console.ReadKey();
                                        if (inf.Key == ConsoleKey.Enter)
                                        {
                                            productRepository.RemoveProductBySerialNumber(serialNumber);
                                            products = productRepository.GetAllProducts();
                                        }
                                    }
                                }
                                if (selectedMagazineStateItem == "Edytuj produkt o podanym numerze seryjnym")
                                {
                                    editProduct(productRepository, products);
                                    products = productRepository.GetAllProducts();
                                }
                                if (selectedMagazineStateItem == "Sortuj produkty po nazwie")
                                {
                                    if (ascendingOrder)
                                    {
                                        products = products.OrderBy(o => o.Name).ToList();
                                        ascendingOrder = false;
                                    }
                                    else
                                    {
                                        products = products.OrderByDescending(o => o.Name).ToList();
                                        ascendingOrder = true;
                                    }

                                }
                                if(selectedMagazineStateItem == "Generuj raport PDF")
                                {
                                    string projectPath = Directory.GetCurrentDirectory();
                                    var mainFolder = Directory.GetParent(Directory.GetParent(projectPath).Parent.FullName);

                                    var pdfFile = Path.Combine(mainFolder.FullName, "PDF/raport.pdf");
                                    var writer = new PdfWriter(pdfFile);
                                    var pdf = new PdfDocument(writer);
                                    var document = new iText.Layout.Document(pdf);

                                    writer.SetCloseStream(false);
                                    document.SetMargins(10f, 20f, 10f, 20f);

                                    for(int i = 0; i < products.Count(); i++)
                                    {
                                        var div = new Div();
                                        var nameTable = new Table(4);
                                        nameTable.AddCell(GetCell((i + 1).ToString() + ".", TextAlignment.LEFT));
                                        nameTable.AddCell(GetCell(products.ToList()[i].Name, TextAlignment.LEFT));
                                        nameTable.AddCell(GetCell("Cena: " + products.ToList()[i].Price.ToString(), TextAlignment.LEFT));
                                        nameTable.AddCell(GetCell("Ilosc: " + products.ToList()[i].Amount.ToString(), TextAlignment.LEFT));
                                        nameTable.SetMarginBottom(10);
                                        div.Add(nameTable);
                                        document.Add(div);
                                    }

                                    writer.SetCloseStream(true);
                                    document.Close();
                                    
                                    viewMessage("Wygenerowano raport.", ConsoleColor.Green);
                                }
                                if (selectedMagazineStateItem == "Wyjście")
                                {
                                    magazineInProgress = false;
                                }
                            }
                        }
                        else if (selectedMenuSeller == "Realizacja zamówień")
                        {
                            ordersInProgress = true;
                            while (ordersInProgress)
                            {
                                Console.Clear();
                                var orders = orderRepository.GetAllOrders();
                                if (orders.Count() <= 0)
                                {
                                    Console.SetCursorPosition(0, 0); Console.WriteLine("Brak zamówień w bazie");
                                    Console.SetCursorPosition(0, 1); kreska(70);
                                    Console.SetCursorPosition(0, 2); Console.WriteLine("Escape - powrót do menu");
                                    Console.SetCursorPosition(0, 3); kreska(70);
                                    ConsoleKeyInfo inf = Console.ReadKey();
                                    if (inf.Key == ConsoleKey.Escape)
                                    {
                                        ordersInProgress = false;
                                    }
                                }
                                else
                                {
                                    Console.Clear(); Console.SetCursorPosition(0, 7); kreska(40); Console.SetCursorPosition(8, 7); Console.WriteLine("Realizacja zamówień");
                                    kreska(100);
                                    Console.WriteLine("Id  |  Email klienta   |     Ilość produktów | Status zamówienia");
                                    kreska(100);
                                    foreach (var order in orders)
                                    {
                                        Console.WriteLine(order.Id + "   " + order.User.Email + "   " + order.Amount
                                             + "       " + order.Status.ToString() + "      ", Color.FromArgb(179, 230, 255));

                                    }
                                    kreska(100);

                                    string selectedOrdersItem = drawMenu(ordersMenu, 30, 1);
                                    if (selectedOrdersItem == "Usuń zamówienie z historii zamówień")
                                    {
                                        Console.Clear(); Console.SetCursorPosition(5, 5); kreska(40); Console.SetCursorPosition(10, 5); Console.WriteLine("Usuwanie pozycji z listy ");
                                        Console.SetCursorPosition(5, 6); kreska(70);
                                        Console.SetCursorPosition(6, 10); Console.WriteLine("Podaj id zamówienia, które chcesz usunąć");
                                        Console.SetCursorPosition(5, 15); kreska(70);
                                        Console.SetCursorPosition(6, 16);

                                        if (!int.TryParse(Console.ReadLine(), out var orderId))
                                        {

                                        }
                                        else
                                        {
                                            Console.SetCursorPosition(5, 17); Console.WriteLine("Wybrana pozycja z listy zostanie usunięta. Czy chcesz ją usunąć? ");
                                            Console.SetCursorPosition(7, 18); Console.WriteLine("Tak - Enter     |    Nie - Backspace");
                                            ConsoleKeyInfo inf = Console.ReadKey();
                                            if (inf.Key == ConsoleKey.Enter)
                                            {
                                                orderRepository.RemoveOrderById(orderId);
                                                Console.Clear();
                                            }
                                            else if (inf.Key == ConsoleKey.Backspace)
                                            {

                                            }
                                        }
                                    }
                                    else if (selectedOrdersItem == "Wyślij zamówienie")
                                    {
                                        Console.Clear(); Console.SetCursorPosition(5, 5); kreska(40); Console.SetCursorPosition(10, 5); Console.WriteLine("Usuwanie pozycji z listy ");
                                        Console.SetCursorPosition(5, 6); kreska(70);
                                        Console.SetCursorPosition(6, 10); Console.WriteLine("Podaj id zamówienia, które chcesz wysłać");
                                        Console.SetCursorPosition(5, 15); kreska(70);
                                        Console.SetCursorPosition(6, 16);
                                        if (!int.TryParse(Console.ReadLine(), out var orderId))
                                        {

                                        }
                                        else
                                        {
                                            Console.SetCursorPosition(5, 17); Console.WriteLine("Wybrane zamówienie zostanie wysłane. Czy jesteś pewien? ");
                                            Console.SetCursorPosition(7, 18); Console.WriteLine("Tak - Enter     |    Nie - Backspace");
                                            ConsoleKeyInfo inf = Console.ReadKey();
                                            if (inf.Key == ConsoleKey.Enter)
                                            {
                                                orderRepository.ChangeOrderStatus(orderId, OrderStatus.Zrealizowane);
                                                Console.Clear();
                                            }
                                            if (inf.Key == ConsoleKey.Backspace)
                                            {

                                            }
                                        }
                                    }
                                    else if (selectedOrdersItem == "Wyjście")
                                    {
                                        ordersInProgress = false;
                                    }

                                }
                            }
                        }
                        else if (selectedMenuSeller == "Wyjście")
                        {
                            sellerMenuInProgress = false;
                            loggedUser = null;
                        }
                    }
                }
                else if (selectedMenuLogin == "Zarejestruj")
                {
                    Console.Clear();

                    Console.SetCursorPosition(0, 1); Console.Write("Imie:"); string name = Console.ReadLine();
                    Console.SetCursorPosition(0, 3); Console.Write("Nazwisko:"); string surname = Console.ReadLine();
                    Console.SetCursorPosition(0, 5); Console.Write("Email:"); string email = Console.ReadLine();
                    Console.SetCursorPosition(0, 7); Console.Write("Miasto:"); string address = Console.ReadLine();
                    Console.SetCursorPosition(0, 9); Console.Write("Hasło:"); string newpassword = Console.ReadLine();
                    Console.SetCursorPosition(0, 11); Console.Write("Powtórz hasło:"); string rnewpassword = Console.ReadLine();

                    if (newpassword != rnewpassword)
                    {
                        viewMessage("Hasła nie zgadzają się. Wciśnij dowolny przycisk.", ConsoleColor.Red);
                    }
                    else
                    {
                        var user = new User
                        {
                            FirstName = name,
                            LastName = surname,
                            Email = email,
                            City = address,
                            Password = newpassword,
                            Role = roleRepository.GetRole(RoleType.Seller)
                        };

                        userRepository.AddEntity(user);
                        viewMessage("Pomyślnie utworzono konto sprzedawcy.", ConsoleColor.Green);
                    }
                }
                else if (selectedMenuLogin == "Wyjście")
                {
                    Console.Clear();
                    sellerMenuInProgress = false;
                    sellerLoginMenuInProgress = false;
                }
            }
        }
    }
}

Cell GetCell(string text, TextAlignment alignment)
{
    Cell cell = new Cell().Add(new Paragraph(text));
    cell.SetPaddingRight(10f);
    cell.SetTextAlignment(alignment);
    cell.SetBorder(Border.NO_BORDER);
    cell.SetFontSize(9);
    return cell;
}

void addProduct(ProductRepository productRepository)
{
    Console.Clear();
    Console.SetCursorPosition(3, 0); kreska(40); Console.SetCursorPosition(20, 0);
    Console.WriteLine("Dane produktu ");
    Console.SetCursorPosition(3, 1); kreska(70);
    string name, size, image, serialnumber, sex;

    Console.SetCursorPosition(3, 3); Console.Write("Nazwa:"); name = Console.ReadLine();
    Console.SetCursorPosition(3, 5); Console.Write("Numer seryjny:"); serialnumber = Console.ReadLine();
    Console.SetCursorPosition(3, 7); Console.Write("Ilosc:");

    if (!int.TryParse(Console.ReadLine(), out var amount))
    {
        viewMessage("Podana ilość towaru nie jest liczbą.", ConsoleColor.Red);
        return;
    }

    Console.SetCursorPosition(3, 9); Console.Write("Cena:");
    var priceInput = Console.ReadLine().Replace(".", ",");
    if (!double.TryParse(priceInput, out var price))
    {
        viewMessage("Podana cena towaru nie jest liczbą.", ConsoleColor.Red);
        return;
    }

    Console.SetCursorPosition(3, 11); Console.Write("Rozmiar (rozmiary oddzielaj przecinkiem):"); size = Console.ReadLine();
    Console.SetCursorPosition(3, 13); Console.Write("Płeć M/K:"); sex = Console.ReadLine();
    if (!(sex.ToString() != "m" ||  sex.ToString() != "k"))
    {
        viewMessage("Podano niepoprawny symbol płci.", ConsoleColor.Red);
        return;
    }

    Console.SetCursorPosition(3, 15); Console.Write("Obrazek:"); image = Console.ReadLine();
    Console.SetCursorPosition(3, 17); kreska(70);
    Console.SetCursorPosition(3, 18); Console.WriteLine("OK - Enter     |    Anuluj - Backspace");

    ConsoleKeyInfo key = Console.ReadKey();
    if (key.Key == ConsoleKey.Enter)
    {
        productRepository.AddProduct(new Product
        {
            Name = name,
            Price = price,
            SerialNumber = serialnumber,
            Amount = amount,
            Size = size,
            Sex = sex,
            Image = image.ToLower().Trim()
        });
        return;
    }

    if (key.Key == ConsoleKey.Backspace)
    {
        return;
    }
}

void editProduct(ProductRepository productRepository, IEnumerable<Product> products)
{

    if (!products.Any())
    {
        Console.SetCursorPosition(0, 0); kreska(40); Console.SetCursorPosition(0, 1); Console.WriteLine("Brak produktów w magazynie");
        Console.SetCursorPosition(0, 2); kreska(70);
        Console.SetCursorPosition(0, 4); Console.WriteLine("Wciśnij dowolny klawisz - powrót do menu");
        ConsoleKeyInfo inf = Console.ReadKey();
        Console.Clear();
        return;
    }

    Console.Clear(); Console.SetCursorPosition(0, 0);
    Console.WriteLine("Podaj numer seryjny produktu, który chcesz edytować");
    Console.SetCursorPosition(0, 1); kreska(70);
    Console.SetCursorPosition(0, 2);
    Console.WriteLine("Nazwa    Numer Seryjny  Liczba Cena Rozmiar  Plec   Obrazek"); kreska(70);
    foreach (var product in products)
    {
        Console.WriteLine(product.Name + "     " + product.SerialNumber
            + " " + product.Amount + " " + product.Price
            + " " + product.Size + " " + product.Sex + " " + product.Image, Color.FromArgb(241, 180, 160));
    }
    var serialNumber = Console.ReadLine();


    var productBySerialNumber = productRepository.GetProductBySerialNumber(serialNumber);
    if (productBySerialNumber == null)
    {
        viewMessage("Nie znaleziono produktu o podanym kodzie seryjnym.", ConsoleColor.Red);
        return;
    }
    else
    {
        kreska(70);
        Console.Clear(); Console.SetCursorPosition(3, 3); kreska(40); Console.SetCursorPosition(8, 3); Console.WriteLine("Dane produktu: ");
        Console.SetCursorPosition(3, 4); kreska(70);
        Console.SetCursorPosition(3, 5); Console.WriteLine("Nazwa: " + productBySerialNumber.Name);
        Console.SetCursorPosition(3, 6); Console.WriteLine("Numer seryjny: " + productBySerialNumber.SerialNumber);
        Console.SetCursorPosition(3, 7); Console.WriteLine("Ilość: " + productBySerialNumber.Amount);
        Console.SetCursorPosition(3, 8); Console.WriteLine("Cena: " + productBySerialNumber.Price);
        Console.SetCursorPosition(3, 9); Console.WriteLine("Rozmiar: " +productBySerialNumber.Size);
        Console.SetCursorPosition(3, 10); Console.WriteLine("Płeć: " + productBySerialNumber.Sex);
        Console.SetCursorPosition(3, 11); Console.WriteLine("Obraz: " + productBySerialNumber.Image);


        Console.SetCursorPosition(3, 12); kreska(70);

        Console.SetCursorPosition(3, 13); Console.Write("Nazwa:");
        string nameEdit = Console.ReadLine();
        Console.SetCursorPosition(3, 14); Console.Write("Numer seryjny:");
        string serialNumberEdit = Console.ReadLine();

        Console.SetCursorPosition(3, 15); Console.Write("Ilość:");
        if (!int.TryParse(Console.ReadLine(), out var amountEdit))
        {
            viewMessage("Podana ilość towaru nie jest liczbą.", ConsoleColor.Red);
            return;
        }

        Console.SetCursorPosition(3, 16); Console.Write("Cena:");
        if (!double.TryParse(Console.ReadLine().Replace(".", ","), out var priceEdit))
        {
            viewMessage("Podana cena towaru nie jest liczbą.", ConsoleColor.Red);
            return;
        }

        Console.SetCursorPosition(3, 17); Console.Write("Rozmiar:");
        var sizeEdit = Console.ReadLine();

        Console.SetCursorPosition(3, 18); Console.Write("Płeć:");
        var sexEdit = Console.ReadLine();

        if (!(sexEdit.ToString() != "m" ||  sexEdit.ToString() != "k"))
        {
            viewMessage("Podano niepoprawny symbol płci.", ConsoleColor.Red);
        }

        Console.SetCursorPosition(3, 19); Console.Write("Obraz:");
        var imageEdit = Console.ReadLine();

        Console.SetCursorPosition(3, 20); kreska(70);
        Console.SetCursorPosition(3, 22); Console.WriteLine("OK - Enter     |    Anuluj - Backspace");
        ConsoleKeyInfo inf = Console.ReadKey();
        if (inf.Key == ConsoleKey.Enter)
        {
            productBySerialNumber.Name = nameEdit;
            productBySerialNumber.SerialNumber = serialNumberEdit;
            productBySerialNumber.Size = sizeEdit;
            productBySerialNumber.Image = imageEdit;
            productBySerialNumber.Price = priceEdit;
            productBySerialNumber.Amount = amountEdit;
            productBySerialNumber.Sex = sexEdit;

            productRepository.UpdateProduct(productBySerialNumber);
        }
        else if (inf.Key == ConsoleKey.Backspace)
        {
            return;
        }
    }
}

static void kreska(int amount)
{
    for (int i = 0; i < amount; i++)
    {
        Console.Write("=");
    }
    Console.WriteLine();
}
string drawMainMenu(List<string> items, int x, int y)
{
    int start = x;
    for (int i = 0; i < items.Count; i++)
    {
        if (i == currentIndex)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(start, y);
            Console.Write(items[i]);
            start += 30;
        }
        else
        {
            Console.SetCursorPosition(start, y);
            Console.Write(items[i]);
            start += 30;
        }
        Console.ResetColor();
    }

    ConsoleKeyInfo ckey = Console.ReadKey();

    if (ckey.Key == ConsoleKey.RightArrow)
    {
        if (currentIndex == items.Count - 1)
        {
            currentIndex = 0;
        }
        else { currentIndex++; }
    }
    else if (ckey.Key == ConsoleKey.LeftArrow)
    {
        if (currentIndex <= 0)
        {
            currentIndex = items.Count - 1;
        }
        else { currentIndex--; }
    }
    else if (ckey.Key == ConsoleKey.Enter)
    {
        var selectedItem = items[currentIndex];
        reset();
        return selectedItem;
    }

    return null;
}
string drawMenu(List<string> items, int x, int y)
{
    int start = y;
    for (int i = 0; i < items.Count; i++)
    {
        if (i == currentIndex)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(x, start);

            Console.WriteLine(items[i]);
            start += 2;
        }
        else
        {

            Console.SetCursorPosition(x, start);
            Console.WriteLine(items[i]);
            start += 2;
        }
        Console.ResetColor();
    }

    ConsoleKeyInfo ckey = Console.ReadKey();

    if (ckey.Key == ConsoleKey.DownArrow)
    {
        if (currentIndex == items.Count - 1)
        {
            currentIndex = 0;
        }
        else { currentIndex++; }
    }
    else if (ckey.Key == ConsoleKey.UpArrow)
    {
        if (currentIndex <= 0)
        {
            currentIndex = items.Count - 1;
        }
        else { currentIndex--; }
    }
    else if (ckey.Key == ConsoleKey.Enter)
    {
        var selectedItem = items[currentIndex];
        reset();
        return selectedItem;
    }

    return null;
}
void reset()
{
    Console.Clear();
    currentIndex = 0;
}
void typePassword()
{
    do
    {
        ConsoleKeyInfo key = Console.ReadKey(true);
        if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
        {
            clientPassword += key.KeyChar;
            Console.Write("*");
        }
        else
        {
            if (key.Key == ConsoleKey.Backspace && clientPassword.Length > 0)
            {
                clientPassword = clientPassword.Substring(0, (clientPassword.Length - 1));
                Console.Write("\b \b");
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                break;
            }
        }
    } while (true);
}

void viewMessage(string message, ConsoleColor color)
{
    Console.Clear();
    Console.ForegroundColor = color;
    Console.WriteLine(message);
    Console.ReadKey();
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.White;
}


string drawMenuProducts(List<string> items)
{
    int start = 1;
    int step = 8;
    for (int i = 0; i < items.Count; i++)
    {
        if(i>=items.Count-3)
        {
            step = 2;
        }
        if (i == currentIndex)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;

            Console.SetCursorPosition(0, start);

            Console.WriteLine(items[i]);
            start += step;
        }
        else
        {
            
            Console.SetCursorPosition(0, start);
            Console.WriteLine(items[i]);
            start += step;            
        }
        Console.ResetColor();
    }

    ConsoleKeyInfo ckey = Console.ReadKey();

    if (ckey.Key == ConsoleKey.DownArrow)
    {
        if (currentIndex == items.Count - 1)
        {
            currentIndex = 0;
        }
        else { currentIndex++; }
    }
    else if (ckey.Key == ConsoleKey.UpArrow)
    {
        if (currentIndex <= 0)
        {
            currentIndex = items.Count - 1;
        }
        else { currentIndex--; }
    }
    else if (ckey.Key == ConsoleKey.Enter)
    {
        var selectedItem = items[currentIndex];
        currentProductIndex = currentIndex;
        reset();
        return selectedItem;
    }

    return null;
}

void viewBasket(List<Product> products, OrderRepository orderRepository, ProductRepository productRepository)
{
    Console.Clear();
    Console.WriteLine("Liczba produktów: " + Basket.productsAmount + "     Łączny koszt: " + Basket.productsCost);
    Console.SetCursorPosition(50, 0);
    Console.WriteLine("Dane do wysyłki:");
    Console.SetCursorPosition(50, 1);
    Console.WriteLine("Imie: " + loggedUser.FirstName);
    Console.SetCursorPosition(50, 2);
    Console.WriteLine("Nazwisko: " + loggedUser.LastName);
    Console.SetCursorPosition(50, 3);
    Console.WriteLine("Email: " + loggedUser.Email);
    Console.SetCursorPosition(50, 4);
    Console.WriteLine("Adres: " + loggedUser.City + "\n");

    if (Basket.products != null)
    {
        for (int i = 0; i < Basket.products.Count(); i++)
        {
            Console.WriteLine(i+1 + ".");
            var productTmp = products.Where(x => x.Id == Basket.products[i].Id).FirstOrDefault();
            if (collection.ContainsKey(productTmp.Image.ToLower()))
            {
                Console.WriteLine(collection[productTmp.Image.ToLower()] + "    " + productTmp.Name + " Ilość: " + Basket.products[i].Amount);
            }
            else
            {
                Console.WriteLine("Brak obrazka    " + productTmp.Name + " Ilość: " + Basket.products[i].Amount);
            }
            
        }
    }

    string selectedBasketItem = drawMenu(basketOptionsMenu, 50, 6);
    if (selectedBasketItem == "Usuń z koszyka")
    {
        try
        {
            if (Basket.products != null && Basket.products.Count > 0)
            {
                basketMenuRemoveInProgress = true;
                while (basketMenuRemoveInProgress)
                {
                    Console.SetCursorPosition(0, 0);
                    Console.Write("Podaj nr pozycji z listy do usunięcia: ");
                    if (!int.TryParse(Console.ReadLine(), out int position))
                    {
                        viewMessage("Podano nieprawidłową wartość", ConsoleColor.Red);
                    }
                    else if (position <= 0 || position > Basket.products.Count)
                    {
                        viewMessage("Podano wartość spoza zakresu", ConsoleColor.Red);
                    }
                    else
                    {
                        Basket.RemoveProduct(position-1);
                    }

                    basketMenuRemoveInProgress = false;
                    Console.Clear();
                }
            }
            else
            {
                Console.Clear();
                viewMessage("Twój koszyk jest pusty.", ConsoleColor.Red);
            }
        }catch (Exception ex)
        {
            viewMessage(ex.Message, ConsoleColor.Red);
        }
    }
    else if (selectedBasketItem == "Zatwierdź zakupy")
    {
        if (Basket.products != null && Basket.products.Count > 0)
        {
            var order = new Order();
            order.User = loggedUser;
            order.Status = OrderStatus.Złożone;
            order.Amount = Basket.productsAmount;
            order.Price = Basket.productsCost;

            foreach (var product in Basket.products)
            {
                orderRepository.CreateOrder(order, product);
                productRepository.SellProduct(product.Id, product.Amount);
            }

            Console.Clear();
            Console.SetCursorPosition(45, 10); kreska(70);
            Console.SetCursorPosition(47, 13); Console.WriteLine("Złożenie zostało złożone!");
            Console.SetCursorPosition(47, 14); Console.WriteLine("Możesz teraz złożyć kolejne zamówienie");
            Console.SetCursorPosition(45, 17); kreska(70);

            Basket.Clear();
        }
        else
        {
            Console.Clear();
            Console.WriteLine("Nie możesz złożyć zamówienia.");
            Console.WriteLine("Twój koszyk jest pusty!");
        }
    }
    else if (selectedBasketItem == "Wyjście")
    {
        basketMenuInProgress = false;
        userBasketInProgress = false;
    }
}

void justShow(List<string> items, int x, int y, int z)
{
    int start = y;
    for (int i = 0; i < items.Count; i++)
    {
        if (z == 1)
        {
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.ForegroundColor = ConsoleColor.Black;
        }
        Console.SetCursorPosition(x, start);
        Console.WriteLine(items[i]);
        start += 2;
    }
    Console.ResetColor();
}