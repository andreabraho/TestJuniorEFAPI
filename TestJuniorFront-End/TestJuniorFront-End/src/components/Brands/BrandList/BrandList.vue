<template>
  <div>

    <table class="table table-striped" v-if="!isLoadingBrands">
        <thead>
            <tr>
            <th scope="col">Id</th>
            <th scope="col">Nome</th>
            <th scope="col">Descrizione</th>
            </tr>
        </thead>
        <tbody>
            <brand-row v-for="brand in brandPageData.brands" :key="brand.id" :brand="brand"></brand-row>
        </tbody>
    </table>

    <page-buttons 
            class="d-flex justify-content-center"
            v-if="!isloadingBrands"
          :page="page"
          :maxPages="brandPageData.totalPages"
          @changePage="changePage"
          ></page-buttons>

  </div>
</template>

<script>
import { MyRepositoryFactory } from "../../../../repositories/MyRepositoryFactory.js";
const BrandRepository = MyRepositoryFactory.get("brands");
import BrandRow from "./components/BrandRow.vue"
import PageButtons from "../../Products/ProductList/Components/PageButtons.vue"
export default {
  data() {
    return {
        /**all data neccessary for the page */
      brandPageData: [],
      /**method to load the page when data are ready */
      isLoadingBrands: false,
      page:1,
      pageSize:10,
    };
  },
  methods: {
      /**method that loads all neccessary page data */
    async load() {
        this.isLoadingBrands = true;
        const { data } = await BrandRepository.getPage(
          this.page, 
          this.pageSize 
          );
        this.brandPageData=data
        this.isLoadingBrands = false;
    },
    /**
     * method launched from event on child component to change the page
     */
    async changePage(num){
        this.page=num;
        await this.load()
    }
  },
  /**
   * on creation load the brand page data for the frist time
   */
  async created() {
      await this.load()
  },
  components:{
      BrandRow,
      PageButtons
  }
};
</script>

<style>
</style>