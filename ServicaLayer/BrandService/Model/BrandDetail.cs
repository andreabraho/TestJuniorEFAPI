using System.Collections.Generic;

namespace ServicaLayer.BrandService.Model
{
    /// <summary>
    /// Rappresents a class that contains all data neccessary for a brand detail page
    /// model used to create the object that will be returned in Produc API brand/Detail/{id}
    /// </summary>
    public class BrandDetail
    {

        /*
         Dettaglio Brand
        L’api prenderà in input l’id di un Brand e restituirà i seguenti dati
        ● Id Brand
        ● Nome del Brand
        ● Numero totale dei Prodotti del Brand
        ● Numero totale delle Richieste informazioni ricevute da tutti i prodotti del Brand
        ● Elenco delle categorie associate ai prodotti del Brand, con i seguenti dati:
            ○ Id Categoria
            ○ Nome Categoria
            ○ Numero di prodotti associati alla categoria
        ● Elenco dei Prodotti con i seguenti dati: (paginazione da gestire lato client)
            ○ Id Prodotto
            ○ Nome del prodotto
            ○ Numero di richieste informazioni ricevute dal prodotto
         */
        /// <summary>
        /// brand id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// brand name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// total of products inserted by the brand
        /// </summary>
        public int TotProducts { get; set; }
        /// <summary>
        /// total of all info requests recived by the brand
        /// </summary>
        public int CountRequestFromBrandProducts { get; set; }
        /// <summary>
        /// list of categories associated to the brand
        /// </summary>
        public IEnumerable<CategoryBrandDetail> AssociatedCategory { get; set; }
        /// <summary>
        /// list of products associated to the brand
        /// </summary>
        public IEnumerable<ProductBrandDetail> Products { get; set; }
    }
    /// <summary>
    /// data neccessary on the page fo each category
    /// </summary>
    public class CategoryBrandDetail
    {
        /// <summary>
        /// category id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// category name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// number of products associated to the category of the brand
        /// </summary>
        public int CountProdAssociatied { get; set; }
    }
    /// <summary>
    /// data neccessary on the page fo each product
    /// </summary>
    public class ProductBrandDetail
    {
        /// <summary>
        /// id product
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// product name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// number of info requests recived by the product
        /// </summary>
        public int CountInfoRequest { get; set; }
    }
}
