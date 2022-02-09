<template>
  <div v-if="!isLoadingProducts">
    <div class="row">
      <div class="col-2 h3 b mt-1">Prodotti</div>
      <div class="col-8"></div>
      <div class="col-2">
        <div class="mt-2">
          <router-link to="/Products/new" class="btn btn-outline-primary"
            ><span class="mt-3">Aggiungi prodotto</span> </router-link
          ><br />
        </div>
      </div>
      <div class="row ml-1 mt-3">
        <div >
          
         <my-table  :tlist="pageData.products" 
                    :brands="pageData.brands"
                    @selectNewBrand="selectNewBrand"
                    @chageOrder="changeOrder"></my-table>
         
        </div>
        <div class="d-flex justify-content-center" >
          
          
          <page-buttons 
          :page="page"
          :probes="testProbe"
          :maxPages="pageData.totalPages"
          @changePage="changePage"
          ></page-buttons>


          
        </div>
        
      </div>
    </div>
  </div>
</template>

<script>
import { MyRepositoryFactory } from "../../../../repositories/MyRepositoryFactory.js";
const ProductRepository = MyRepositoryFactory.get("products");
import MyTable from "./Components/Table.vue"
import PageButtons from "./Components/PageButtons.vue"
export default {
  name: "ProductList",
  data() {
    return {
      testProbe:{name:'Andrea',surname:'Braho',marks:[1],skills:[{language:'C#',level:'ciuccio'},{language:'Javascript',level:'Donkey'}]},
      page: 1,
      pageSize: 10,
      /** rappresents all the data coming from the api */
      pageData: null,
      /**
       * variable used to load the compoents whenever tha data are ready
       */
      isLoadingProducts: true,
      /**value that rappresents the id of the brand to filter on */
      brandSelected:0,
      /**value 1(brand name),2(product name),3(price) rappresents the order of the list */
      orderBy:0,
      isAsc:true,
    };
  },
  methods: {
    /**
     * called at the creation of the component loads the page neccessary data
     */
    async load() {
      this.isLoadingProducts = true;
      const { data } = await ProductRepository.getPage(
        this.page,
        this.pageSize,
        this.brandSelected,
        this.orderBy,
        this.isAsc
      );
      this.pageData = data;

      this.isLoadingProducts = false;

    },
    /**
     * function that updates the page data whenever search/filter/order data changes
     */
    async update() {

      const { data } = await ProductRepository.getPage(
        this.page,
        this.pageSize,
        this.brandSelected,
        this.orderBy,
        this.isAsc
      );
      this.pageData = data;
      

    },
    /**
     * method that updated the brand filter 
     * and update the data in the page
     * triggered by event from child components
     ** @id rappresents the id of the brand to filter on
     */
    async selectNewBrand(id){
      this.brandSelected=id
      this.page=1
      await this.update()


    },
    /**
     * method that updated the order filter 
     * and update the data in the page
     * triggered by event from child components
     ** @orderBy having value 1,2,3  raprresenting brandName,productName,price order
     ** @isAsc boolean value rappresenting if the order is ascending or descending
     */
    async changeOrder(orderBy,isAsc){
      this.orderBy=orderBy
      this.isAsc=isAsc
      this.page=1
      await this.update()
    },
    /**
     * method that updated the page nuber
     * and update the data in the page
     * triggered by event from child components
     **@num input data rappresentin the new page number coming from child event
     */
    async changePage(num){
      this.page=num
      await this.update();
    }
  },
  computed: {},
  components: {
    MyTable,
    PageButtons
  },
  async created() {
    await this.load();
  },
  async mounted() {},
};
</script>

<style scoped>

</style>
