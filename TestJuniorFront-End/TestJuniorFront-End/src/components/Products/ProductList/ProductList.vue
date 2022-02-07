<template>
  <div>
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
        <div v-if="!isLoadingProducts">
          
         <my-table  :tlist="pageData.products" 
                    :brands="pageData.brands"
                    @selectNewBrand="selectNewBrand"
                    @chageOrder="chageOrder"></my-table>
         
        </div>
        <div class="d-flex justify-content-center" v-if="!isLoadingProducts">
          <button type="button" 
                  class="ml-1" 
                  @click="previousPage()"
                  :class="[page==1?'disabled btn btn-outline-secondary':'btn btn-outline-primary']">Indietro</button>
          
          


          <button type="button" 
                  class="ml-1" 
                  @click="nextPage()"
                  :class="[page==pageData.totalPages?'disabled btn btn-outline-secondary':'btn btn-outline-primary']">Avanti</button>
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
      page: 1,
      pageSize: 10,
      pageData: null,
      isLoadingProducts: true,
      brandSelected:0,
      orderBy:0,
      isAsc:false,
    };
  },
  methods: {
    async load() {
      this.isLoadingProducts = true;
      const { data } = await ProductRepository.getPage(
        this.page,
        this.pageSize,
        this.brandSelected,
        this.orderBy,
        this.isAsc
      );

      this.isLoadingProducts = false;
      this.pageData = data;
      console.log(data)

    },
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
    async setPage(val) {
      this.page = val;
      this.update();
    },
    async nextPage() {
      if (this.page < this.pageData.totalPages) {
        this.page = this.page + 1;
        this.update();
      }
    },
    async previousPage() {
      if (this.page > 1) {
        this.page = this.page - 1;
        this.update();
      }
    },
    
    selectNewBrand(id){
      this.brandSelected=id
      this.page=1
      this.update()

    },
    chageOrder(orderBy,isAsc){
      this.orderBy=orderBy
      this.isAsc=isAsc
      this.page=1
      this.update()
    }
  },
  computed: {},
  components: {
    MyTable,
    PageButtons
  },
  created() {
    this.load();
  },
  async mounted() {},
};
</script>

<style scoped>
</style>
