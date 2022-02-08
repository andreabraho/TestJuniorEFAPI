<template>
<div>
    detail lead page ID:{{idIR}}
</div>
</template>

<script>
import { MyRepositoryFactory } from "../../../../repositories/MyRepositoryFactory.js";
const IRRepository = MyRepositoryFactory.get("inforequests");
export default {
  data() {
    return {
        idIR:this.$route.params.id,
        infoRequest:null,
        isLoadingIR:true
    };
  },
  methods:{
    async load(){
            this.isLoadingIR=true;
            const { data } = await IRRepository.getInfoRequest(
            this.idIR
            );
            this.infoRequest=data
            this.isLoadingIR=false;

        }
  },
  created(){
    this.load()
  }
};
</script>