<template>
  <div>
    <div class="row">
      <div class="col-2">Prodotti</div>
      <div class="col-8">vuoto</div>
      <div class="col-2">
        <div class="mt-2">
          <router-link to="/Products/new" class="btn btn-outline-primary"
            ><span class="mt-3">Aggiungi prodotto</span> </router-link
          ><br />
        </div>
      </div>
      <div class="row ml-1 mt-3">
        <div v-if="!isLoadingProducts">
          <b-table striped hover :items="pageData.products"></b-table>
        </div>
        <button @click="nextPage()">next page</button>
        <button @click="previousPage()">previous page</button>
      </div>
    </div>
  </div>
</template>

<script>
import { MyRepositoryFactory } from "../../../../repositories/MyRepositoryFactory.js";
const ProductRepository = MyRepositoryFactory.get("products");

export default {
  name: "ProductList",
  data() {
    return {
      page: 1,
      pageSize: 10,
      pageData: null,
      isLoadingProducts: true,
    };
  },
  methods: {
    async load() {
      this.isLoadingProducts = true;
      const { data } = await ProductRepository.getPage(
        this.page,
        this.pageSize
      );

      this.isLoadingProducts = false;
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
    async update() {
      const { data } = await ProductRepository.getPage(
        this.page,
        this.pageSize
      );
      this.pageData = data;
    },
  },
  computed: {},
  components: {},
  created() {
    this.load();
  },
  async mounted() {},
};
</script>

<style scoped>
</style>
