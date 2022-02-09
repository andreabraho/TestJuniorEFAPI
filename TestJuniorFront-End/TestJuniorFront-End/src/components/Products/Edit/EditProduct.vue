
</script>
<template>
  <div class="row">
      <div class="col-1"></div>
      <div class="col-7">
          <div class="d-flex justify-content-start mt-3"> 
            <span class="h2 ">Update Product</span>
          </div>

          <form @submit.prevent="sendData" class="mt-5 mb-5">
            <div class="form-group row mt-2">
                <label >Name</label>
                <input type="text" class="form-control" v-model="form.product.name">
            </div>
            <div class="form-group row mt-2">
                <label >Short Description</label>
                <input type="text" class="form-control" v-model="form.product.shortDescription">
            </div>
            
            <div class="form-group row mt-2">
                <label >Description</label>
                <textarea class="form-control"  
                        rows="7"
                        v-model="form.product.description"></textarea>
            </div>
            <div class="form-group row mt-2">
                <label >Price</label>
                <input type="number" class="form-control" v-model="form.product.price">
            </div>
            
            <div class="row mt-2">
                <label for="">Categories</label>
                <select class="selectpicker " 
                        multiple  
                        v-model="form.categoriesIds"
                        size="10">
                    <option selected>Select Categories</option>

                    <option 
                    v-for="item in dataForUpdate.allCategories"
                    :key="item.id"
                    :value="item.id"
                    selected>{{item.name}}
                    </option>
                
                </select>
            </div>

            <div class="d-flex justify-content-end row mt-2">
            <button class="btn btn-outline-primary mt-2 " >Invio</button>
            </div>
        </form>

      </div>
      <div class="col-4 "></div>
    
  </div>
</template>

<script>
import { MyRepositoryFactory } from "../../../../repositories/MyRepositoryFactory.js";
const ProductRepository = MyRepositoryFactory.get("products");
export default {
    data(){
        return{
            form:{
                product:{
                    id:this.$route.params.id,
                    name:"",
                    shortDescription:"",
                    description:"",
                    price:0,
                    brandId:0
                },
                categoriesIds:[],
                
            },
            dataForUpdate:null,
            isLoading:false,
            idProduct:this.$route.params.id,
            
        }
    },
    methods:{
        async load(){
            this.isLoading=true
            const { data } = await ProductRepository.getDataForUpdate(this.$route.params.id);
            this.dataForUpdate=data
            this.updateFormData(data)
        },
        async sendData(){
            
            await ProductRepository.editProduct(this.form);
            this.$router.push("/products/"+this.idProduct)
        },
        updateFormData(data){
            this.form.product.name=data.product.name
            this.form.product.shortDescription=data.product.shortDescription
            this.form.product.description=data.product.shortDescription
            this.form.product.price=data.product.price
            this.form.product.brandId=data.product.brandId

            this.form.categoriesIds=data.categoriesAssociated
            this.isLoading=false

        }
    },
    async created(){
        await this.load()
    }
}
</script>

<style scoped>
.myselect{
    height: 200px;
}
</style>