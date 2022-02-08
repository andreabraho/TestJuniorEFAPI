<template>
  <div>
    <my-table  :tlist="pageData.products" 
                    :brands="pageData.brands"
                    @selectNewBrand="selectNewBrand"
                    @chageOrder="chageOrder"></my-table>

  </div>
</template>

<script>
import { MyRepositoryFactory } from "../../../../repositories/MyRepositoryFactory.js";
const IRRepository = MyRepositoryFactory.get("inforequests");
import MyTable from "../../Products/ProductList/Components/Table.vue"
export default {
  data() {
    return {
      pageData: null,
      isLoadingIR: false,
      page:1,
      pageSize:10,
      brandSelected:0,
    }
  },
  methods: {
    async load() {
      this.isLoadingIR = true;
      const { data } = await IRRepository.getPage(
        this.page,
        this.pageSize,
        this.brandSelected
      );
      this.pageData = data;
      this.isLoadingIR = false;
    
    },
    selectNewBrand(id){
      
    }
  },
  async created(){
    await this.load()
  },
  components:{
    MyTable
  }
  
};
</script>