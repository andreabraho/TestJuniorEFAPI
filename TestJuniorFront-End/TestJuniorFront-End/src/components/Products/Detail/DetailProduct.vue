<template>
    <div>
        detail prod ID: {{idPorduct}}
    </div>
</template>

<script>
import { MyRepositoryFactory } from "../../../../repositories/MyRepositoryFactory.js";
const ProductRepository = MyRepositoryFactory.get("products");
export default ({
    data(){
        return {
            idPorduct:this.$route.params.id,
            product:null,
            isLoadingProduct:true
        }
    },
    methods:{
        async load(){
            this.isLoadingProduct=true;
            const { data } = await ProductRepository.getProduct(
            this.idPorduct
            );
            this.product=data
            this.isLoadingProduct=false;

        }
    },
    created(){
        this.load()
    }
})
</script>


