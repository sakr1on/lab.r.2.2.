using System.Xml;

/// <summary>
/// Класс, представляющий товар в заказе.
/// </summary>
public class Item
{
    /// <summary>
    /// Наименование товара.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Количество единиц товара в заказе.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Цена за единицу товара.
    /// </summary>
    public decimal Price { get; set; }
}

/// <summary>
/// Класс, представляющий заказ со списком товаров.
/// </summary>
public class Order
{
    /// <summary>
    /// Информация о доставке.
    /// </summary>
    public class ShipTo
    {
        /// <summary>
        /// Имя получателя заказа.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Улица доставки.
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// Адрес доставки.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Страна доставки.
        /// </summary>
        public string Country { get; set; }
    }

    /// <summary>
    /// Информация о доставке заказа.
    /// </summary>
    public ShipTo ShipToInfo { get; set; }

    /// <summary>
    /// Список товаров в заказе.
    /// </summary>
    public List<Item> Items { get; set; }
}

/// <summary>
/// Статический класс для преобразования XML-документа в объект класса <see cref="Order"/>.
/// </summary>
public static class XmlToOrderConverter
{
    /// <summary>
    /// Метод для преобразования XML-строки в объект класса <see cref="Order"/>.
    /// </summary>
    /// <param name="xmlString">XML-строка, представляющая заказ.</param>
    /// <returns>Объект класса <see cref="Order"/>, созданный на основе XML-документа.</returns>
    public static Order Convert(string xmlString)
    {
        Order order = new Order();
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlString);

        // Разбор информации о доставке
        XmlNode shipToNode = xmlDoc.SelectSingleNode("/shipOrder/shipTo");
        order.ShipToInfo = new Order.ShipTo
        {
            Name = shipToNode.SelectSingleNode("name").InnerText,
            Street = shipToNode.SelectSingleNode("street").InnerText,
            Address = shipToNode.SelectSingleNode("address").InnerText,
            Country = shipToNode.SelectSingleNode("country").InnerText
        };

        // Разбор товаров
        order.Items = new List<Item>();
        XmlNodeList itemNodes = xmlDoc.SelectNodes("/shipOrder/items/item");
        foreach (XmlNode itemNode in itemNodes)
        {
            Item item = new Item
            {
                Title = itemNode.SelectSingleNode("title").InnerText,
                Quantity = int.Parse(itemNode.SelectSingleNode("quantity").InnerText),
                Price = decimal.Parse(itemNode.SelectSingleNode("price").InnerText)
            };
            order.Items.Add(item);
        }

        return order;
    }
}

/// <summary>
/// Главный класс программы для демонстрации работы созданных классов.
/// </summary>
class Program
{
    /// <summary>
    /// Точка входа в программу.
    /// </summary>
    static void Main()
    {
        string xmlString = @"
            <shipOrder>
                <shipTo>
                    <name>Tove Svendson</name>
                    <street>Ragnhildvei 2</street>
                    <address>4000 Stavanger</address>
                    <country>Norway</country>
                </shipTo>
                <items>
                    <item>
                        <title>Empire Burlesque</title>
                        <quantity>1</quantity>
                        <price>10.90</price>
                    </item>
                    <item>
                        <title>Hide your heart</title>
                        <quantity>1</quantity>
                        <price>9.90</price>
                    </item>
                </items>
            </shipOrder>";

        Order order = XmlToOrderConverter.Convert(xmlString);
         
        Console.WriteLine($"Информация о доставке:");
        Console.WriteLine($"Имя: {order.ShipToInfo.Name}");
        Console.WriteLine($"Улица: {order.ShipToInfo.Street}");
        Console.WriteLine($"Адрес: {order.ShipToInfo.Address}");
        Console.WriteLine($"Страна: {order.ShipToInfo.Country}");

        Console.WriteLine("\nТовары:");
        foreach (var item in order.Items)
        {
            Console.WriteLine($"Наименование: {item.Title}, Количество: {item.Quantity}, Цена: {item.Price}");
        }
    }
}