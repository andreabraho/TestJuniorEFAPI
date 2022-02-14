<template>
  <div v-if="!isLoadingIR">

    <div class="row mt-3 h2 header">
      <p>Leads </p>
    </div>



    <my-table  :tlist="pageData.infoRequests" 
                    :brands="pageData.brands"
                    @selectNewBrand="selectNewBrand"
                    @changeOrder="changeOrder"
                    @changeSearch="changeSearch"
                    :search="search"
                    :selectedBrand="brandSelected"
                    class="text"
                    ></my-table>




      
      <page-buttons class="d-flex justify-content-center"
      v-if="!isLoadingIR" 
      :page="page"
      :maxPages="pageData.totalPages"
      @changePage="changePage"
      
      
      ></page-buttons>
      

  </div>
</template>

<script>
import { MyRepositoryFactory } from "../../../../repositories/MyRepositoryFactory.js";
const IRRepository = MyRepositoryFactory.get("inforequests");
import MyTable from "./Components/Table.vue"
import PageButtons from "../../Generic/PageButtons.vue"
export default {
  props:{
    
  },
  data() {
    return {
      /**contains all data coming from api */
      pageData: null,
      /**tell if the page is loading */
      isLoadingIR: true,
      page:1,
      pageSize:10,
      brandSelected:0,
      search:null,
      isAsc:false,
      productId:0
    }
  },
  methods: {
    /**
     * called at the creation of the component loads the page neccessary data
     */
    async load() {
      this.isLoadingIR = true;
      if(this.$route.params.productId!=undefined)
      this.productId=this.$route.params.productId
      

      const { data } = await IRRepository.getPage(
        this.page,
        this.pageSize,
        this.brandSelected,
        this.search,
        this.isAsc,
        this.productId
      );
      this.pageData = data;
      this.isLoadingIR = false;
    },
    /**
     * function that updates the page data whenever search/filter/order data changes
     */
    async update() {

      const { data } = await IRRepository.getPage(
        this.page,
        this.pageSize,
        this.brandSelected,
        this.search,
        this.isAsc,
        this.productId

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
     ** @isAsc boolean value rappresenting if the order is ascending or descending
     */
    async changeOrder(isAsc){
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

    },
    /**method that update pageData based on new search
     ** called from child component when a new search is done
     ** @search new search 
     */
    async changeSearch(search){
      this.search=search
      this.page=1
      await this.update()
    }
  },
  async created(){
    await this.load()
    this.$emit("setActiveLink",3)
  },
  components:{
    MyTable,
    PageButtons
  }
  
};
</script>

<style scoped>
.text{
  word-wrap: break-word;
}
.header{
  border-bottom: 1px solid rgb(216, 216, 216);
  padding-bottom: 1rem;
  margin-bottom: 2rem;
}
</style>