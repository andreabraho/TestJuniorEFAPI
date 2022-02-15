<template>
  <div v-if="!isLoadingProducts">

    <!-- Modal -->
    <Modal
      v-show="isModalVisible"
      @close="closeModal"
      >
      <template v-slot:header>
        This is a new modal header.
      </template>

      <template v-slot:body>
        This is a new modal body.
      </template>

      <template v-slot:footer>
        This is a new modal footer.
      </template>
    </Modal>

    
    <div
      class="modal fade"
      id="staticBackdrop"
      data-bs-backdrop="static"
      data-bs-keyboard="false"
      tabindex="-1"
      aria-labelledby="staticBackdropLabel"
      aria-hidden="true"
    >
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="staticBackdropLabel">
              Stai Eliminando Un Prodotto
            </h5>
            <button
              type="button"
              class="btn-close"
              data-bs-dismiss="modal"
              aria-label="Close"
            ></button>
          </div>
          <div class="modal-body">
            Sei sicuro di voler eliminare il prodotto?
          </div>
          <div class="modal-footer">
            <button
              type="button"
              class="btn btn-primary"
              data-bs-dismiss="modal"
            >
              Chiudi
            </button>
            <button
              type="button"
              class="btn btn-danger"
              data-bs-dismiss="modal"
              @click.stop="deleteProduct"
            >
              Elimina
            </button>
          </div>
        </div>
      </div>
    </div>
    
    <div class="row">
      <page-title title="Prodotti">
      <template v-slot:button>
        <router-link to="/Products/new" 
                    class="btn btn-outline-primary">
          <span class="mt-3">Aggiungi prodotto</span> 
        </router-link>
      </template>
      </page-title>

      <div class="row ml-1 mt-3">
        <div>
          <my-table
            :tlist="pageData.products"
            :brands="pageData.brands"
            @selectNewBrand="selectNewBrand"
            @chageOrder="changeOrder"
            @deleteProd="deleteProd"
          ></my-table>
        </div>

        <div class="">
          <page-buttons
            class="d-flex justify-content-center"
            :page="page"
            :probes="testProbe"
            :maxPages="pageData.totalPages"
            @changePage="changePage"
          ></page-buttons>
        </div>
      </div>
    </div>
  </div>
  <div v-else><!-- skeleton zone-->
    <div class="mt-5 mb-5">
      <b-skeleton animation="wave" width="85%" class="mb-5"></b-skeleton>
      <b-skeleton animation="wave" width="55%"></b-skeleton>
      <b-skeleton animation="wave" width="70%" class="mb-5"></b-skeleton>
      <b-skeleton-table
        :rows="10"
        :columns="5"
        :table-props="{ bordered: true, striped: true }"
        
      ></b-skeleton-table>
    </div>
  </div>
</template>

<script>
import { MyRepositoryFactory } from "../../../../repositories/MyRepositoryFactory.js";
const ProductRepository = MyRepositoryFactory.get("products");
import MyTable from "./Components/Table.vue";
import PageButtons from "../../Generic/PageButtons.vue";
import PageTitle from "../../Generic/PageTitle.vue"
//import ModalDelete from "../../Generic/PageTitle.vue"
import Modal from "../../Generic/Modal.vue"
export default {
  name: "ProductList",
  props: {},
  data() {
    return {
      showDialog:false,
      isModalVisible:false,
      page: 1,
      pageSize: 25,
      /** rappresents all the data coming from the api */
      pageData: null,
      /**
       * variable used to load the compoents whenever tha data are ready
       */
      isLoadingProducts: true,
      /**value that rappresents the id of the brand to filter on */
      brandSelected: 0,
      /**value 1(brand name),2(product name),3(price) rappresents the order of the list */
      orderBy: 0,
      isAsc: true,
      /**id prod to delete set from child component tableRow on delete click */
      idProdToDelete: 0,
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
    async selectNewBrand(id) {
      this.brandSelected = id;
      this.page = 1;
      await this.update();
    },
    /**
     * method that updated the order filter
     * and update the data in the page
     * triggered by event from child components
     ** @orderBy having value 1,2,3  raprresenting brandName,productName,price order
     ** @isAsc boolean value rappresenting if the order is ascending or descending
     */
    async changeOrder(orderBy, isAsc) {
      this.orderBy = orderBy;
      this.isAsc = isAsc;
      this.page = 1;
      await this.update();
    },
    /**
     * method that updated the page nuber
     * and update the data in the page
     * triggered by event from child components
     **@num input data rappresentin the new page number coming from child event
     */
    async changePage(num) {
      this.page = num;
      await this.update();
    },
    /**called from child component set prod to delete id */
    deleteProd(id) {
      this.idProdToDelete = id;
    },
    /**method that delete a product effectively */
    async deleteProduct() {
      const res = await ProductRepository.deleteProduct(this.idProdToDelete);
      await this.update();

      if (res.data) this.$toast.top("Deleted succesfully");
      if (!res.data) this.$toast.top("Not deleted upsi");
    },
  },
  computed: {},
  components: {
    MyTable,
    PageButtons,
    PageTitle,
    //ModalDelete,
    Modal
  },
  async created() {
    await this.load();
    this.$emit("setActiveLink", 1);
  },
  async mounted() {},
};
</script>

<style scoped>

</style>
