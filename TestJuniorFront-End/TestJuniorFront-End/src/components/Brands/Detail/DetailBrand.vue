<template>
    <div>
        detail brand ID:{{idBrand}}
    </div>
</template>


<script>
import { MyRepositoryFactory } from "../../../../repositories/MyRepositoryFactory.js";
const BrandRepository = MyRepositoryFactory.get("brands");
export default ({
    data(){
        return {
            idBrand:this.$route.params.id,
            brand:null,
            isLoadingBrand:true
        }
    },
    methods:{
        async load(){
            this.isLoadingBrand=true;
            const { data } = await BrandRepository.getBrand(
            this.idBrand
            );
            this.brand=data
            this.isLoadingBrand=false;
        }
    },
    created(){
        this.load()
    }
})
</script>
