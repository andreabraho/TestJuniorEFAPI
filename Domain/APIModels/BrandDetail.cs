using Domain;
using System.Collections.Generic;

namespace Domain.APIModels
{
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
        public int Id { get; set; }
        public string Name { get; set; }
        public int TotProducts { get; set; }
        public int CountRequestFromBrandProducts { get; set; }
        public List<CategoryBrandDetail> AssociatedCategory { get; set; }=new List<CategoryBrandDetail>();
        public List<ProductBrandDetail> Products { get; set; }=new List<ProductBrandDetail>();
    }
    public class CategoryBrandDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountProdAssociatied { get; set; }
    }
    public class ProductBrandDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountInfoRequest { get; set; }
    }
}
