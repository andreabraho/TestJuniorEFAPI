<template>
  <div v-if="!isLoadingBrands">

    
  <!-- Modal -->
  <div class="modal fade" id="staticBackdrop" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="staticBackdropLabel">Stai Eliminando Un Brand</h5>
          <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
        </div>
        <div class="modal-body">
          Sei sicuro di voler eliminare il brand?
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-primary" data-bs-dismiss="modal">Chiudi</button>
          <button type="button" class="btn btn-danger" data-bs-dismiss="modal" @click.stop="effDeleteBrand">Elimina</button>
        </div>
      </div>
    </div>
  </div>

    <page-title title="Brand">
      <template v-slot:button>
        <router-link to="/Brands/new" class="btn btn-outline-primary"
            ><span class="mt-3">Aggiungi brand</span> </router-link>
      </template>
    </page-title>

    <table class="table table-striped" >
        <thead>
            <tr>
            <th scope="col">Id</th>
            <th scope="col">Nome</th>
            <th scope="col">Descrizione</th>
            <th></th>
            </tr>
        </thead>
        <tbody> 
            <brand-row v-for="brand in brandPageData.brands" 
                        :key="brand.id" 
                        :brand="brand"
                        @deleteBrand="deleteBrand"></brand-row>
        </tbody>
    </table>

    <page-buttons 
          class="d-flex justify-content-center"
          :page="page"
          :maxPages="brandPageData.totalPages"
          @changePage="changePage"
          ></page-buttons>

  </div>
  <div v-else>
    
    <my-skeleton-table message="Brand"></my-skeleton-table>
  </div>
</template>

<script>
import { MyRepositoryFactory } from "../../../../repositories/MyRepositoryFactory.js";
const BrandRepository = MyRepositoryFactory.get("brands");
import BrandRow from "./components/BrandRow.vue"
import PageButtons from "../../Generic/PageButtons.vue"
import MySkeletonTable from "../../Generic/MySkeletonTable.vue"
import PageTitle from "../../Generic/PageTitle.vue"
export default {
  data() {
    return {
        /**all data neccessary for the page */
      brandPageData: [],
      /**method to load the page when data are ready */
      isLoadingBrands: true,
      page:1,
      pageSize:10,
      idBrandToDelete:0
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
    },
    /**method called from child to update the id of the brand to delete */
    deleteBrand(id){
      this.idBrandToDelete=id
    },
    async effDeleteBrand(){
      var res=await BrandRepository.deleteBrand(this.idBrandToDelete)
      await this.load()

      if(res.data)
        this.$toast.top("Deleted succesfully")
      if(!res.data)
        this.$toast.top("Not deleted upsi")
    }

  },
  /**
   * on creation load the brand page data for the frist time
   */
  async created() {
      await this.load()
    this.$emit("setActiveLink",2)

  },
  components:{
      BrandRow,
      PageButtons,
      MySkeletonTable,
      PageTitle
  }
};
</script>

<style scoped>

</style>