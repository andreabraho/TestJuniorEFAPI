<template>
<div v-if="!isLoadingIR" class="row">
  <div class="col-2"></div>
  <div class="col-8">
    <div class="row mt-3">
      <p class="h4">Richiesta informazioni di {{infoRequest.name}} {{infoRequest.lastName}}<br>
        per il Prodotto {{infoRequest.productIRDetail.name}} di {{infoRequest.productIRDetail.brandName}}
      </p>
    </div>
    <div class="row mt-4">
      <p>
      <b>Dati del richiedente</b><br>
      {{infoRequest.name}} {{infoRequest.lastName}}<br>
      {{infoRequest.location}}
      
      </p>
    </div>

    <div class="row mt-3">
      <p>
        <b>Richiesta inviata dal utente:</b><br>
        {{infoRequest.requestText}}
      </p>
    </div>

    <div class="row mt-3">
      <p><b>Risposte/Commenti alla richiesta</b></p>
    </div>

    <div class="card mb-4" 
          v-for="item in replyPerPage"
          :key="item.id">
      <h5 class="card-header">{{data(item.date)}}  -  {{item.user}}</h5>
      <div class="card-body">
        <p class="card-text">{{item.replyText}}</p>
      </div>
    </div>

    <div class="row">
                <page-buttons 
                    class="d-flex justify-content-center"
                    :page="page"
                    :maxPages="Math.ceil(infoRequest.irModelReplies.length/pageSize)"
                    @changePage="changePage"
                    ref="pagingComponent"
                    ></page-buttons>
            </div>

  </div>
  <div class="col-2"></div>


</div>
</template>

<script>
import { MyRepositoryFactory } from "../../../../repositories/MyRepositoryFactory.js";
const IRRepository = MyRepositoryFactory.get("inforequests");
import PageButtons from "../../Products/ProductList/Components/PageButtons.vue"
export default {
  data() {
    return {
      /**info request id taken from route */
        idIR:this.$route.params.id,
        /**data coming from api */
        infoRequest:null,
        isLoadingIR:true,
        page:1,
        pageSize:2
    };
  },
  computed:{
    
  },
  methods:{
    /**load the neccessary data for the page from api */
    async load(){
            this.isLoadingIR=true;
            const { data } = await IRRepository.getInfoRequest(
            this.idIR
            );
            this.infoRequest=data
            this.isLoadingIR=false;

        },
        /**method to get the data in the correct format */
    data(date){
      let newdata=date.split("T")
      return newdata[0]
    },
    /**method to change page called from child component */
    changePage(num){
      this.page=num
    }
  },
  computed:{
    /**list of reply for the current page */
    replyPerPage(){
      let l=this.infoRequest.irModelReplies.length;
      let start=(this.page-1)*this.pageSize;
      //let end=start+this.pageSize>l?start+this.pageSize:l;
      let end=0
      if(start+this.pageSize<l)
          end=start+this.pageSize
      else
          end=l

      let array=this.infoRequest.irModelReplies.slice(start,end);
      return array
    }
  },
  async created(){
    await this.load()
  },
  components:{
    PageButtons
  }
};
</script>

<style scoped>
.card-header{
  background-color: rgb(239, 245, 231);
}
.card-body{
  color: rgb(44, 73, 0);
}
</style>