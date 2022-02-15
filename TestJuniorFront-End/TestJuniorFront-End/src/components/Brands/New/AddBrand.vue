<template>
  <div v-if="!isLoading" class="row">

    <div class="row mb-3 mt-3">
      <div class="col-2"></div>
      
      <div class="col-8"><p class="h2">Create New Brand</p></div>
      <div class="col-2"></div>
    </div>

    <div class="col-2">

    </div>
    <div class="col-8">
      
      <form @submit.prevent="sendData" >
        <div class="row">
         <div class="mb-3">
            <label for="exampleInputEmail1" class="form-label">Email address</label>
            <input type="email" 
                  class="form-control" 
                  :class="[errors.email!=null?'error-input':'']"
                  v-model="form.account.email"
                  @blur="validateEmail">
            <div class="form-text">We'll never share your email with anyone else.<br>
            <span class="text-danger">{{errors.email}}</span></div>
          </div>
          <div class="mb-3">
            <label for="exampleInputPassword1" class="form-label">Password</label>
            <input type="password"
                  class="form-control"
                  :class="[errors.password!=null?'error-input':'']"
                  @blur="isPasswordValid"
                  v-model="form.account.password">
            <div class="form-text text-danger">{{errors.password}}</div>
            
          </div>
          <div class="mb-3">
            <label for="exampleFormControlInput1" class="form-label">Brand Name</label>
            <input type="text" 
                    class="form-control"  
                    :class="[errors.brandName!=null?'error-input':'']"
                    v-model="form.brand.brandName"
                    @blur="isBrandNameValid">
            <div class="form-text text-danger">{{errors.brandName}}</div>
            
          </div>
          <div class="mb-3">
            <label for="exampleFormControlTextarea1" class="form-label">Brand Description</label>
            <textarea class="form-control"
                      :class="[errors.brandDescription!=null?'error-input':'']"
                      @blur="isBrandDescriptionValid"
                      rows="3" 
                      v-model="form.brand.description"></textarea>
            <div class="form-text text-danger">{{errors.brandDescription}}</div>
            
          </div>
          </div>
          <!-- ---------------------------------------------------------------------------------- -->
          
          <div v-for="(product,index) in form.prodsWithCats"
                class="border bg-light mt-5 row p-2"
                :key="index"
                >

          <div>
            <p class="h3 mt-2 mb-3 h-25 position-relative"> 
              Product #{{index+1}}
              <i class="btn btn-danger bi bi-file-earmark-x position-absolute top-0 end-0  me-2"
              @click="removeProduct(index)"></i></p>
          </div>

          <div class="row mt-2">
            <label >Name</label>
            <input type="text" 
                  class="form-control"
                  :class="[product.errors.name!=null?'error-input':'']"
                  @blur="isProductNameValid(product)"
                  v-model="product.product.name">
            <div class="form-text text-danger">{{product.errors.name}}</div>
            
            </div>
            <div class="form-group row mt-2">
                <label >Short Description</label>
                <input type="text" 
                    class="form-control" 
                    :class="[product.errors.shortDescription!=null?'error-input':'']"
                    v-model="product.product.shortDescription"
                    @blur="isProductShortDescriptionValid(product)">
            <div class="form-text text-danger">{{product.errors.shortDescription}}</div>

            </div>
            
            <div class="form-group row mt-2">
                <label >Description</label>
                <textarea class="form-control"  
                        rows="7"
                        v-model="product.product.description"></textarea>
            <div class="form-text text-danger">{{product.errors.description}}</div>

            </div>
            <div class="form-group row mt-2">
                <label >Price</label>
                <input type="number" 
                      class="form-control"
                      :class="[product.errors.price!=null?'error-input':'']"
                      v-model="product.product.price" 
                      step=".01"
                      @blur="isProductPriceValid(product)">
            <div class="form-text text-danger">{{product.errors.price}}</div>

            </div>

            <div class="row ">
              <div class="m-3 checboxes "
                    :class="[product.errors.categories!=null?'error-input-catbox':'']"
                    >
                  <div class="form-check form-check-inline col mt-3"
                  v-for="item in catForSelect"
                  :key="item.id">
                    <input class="form-check-input" 
                          type="checkbox" 
                          :value="item.id" 
                          v-model="product.categoriesIds"
                          @blur="isProductCategoriesValid(product)">
                    <label class="form-check-label" >{{item.name}}</label>
                  </div>
              <div class="form-text text-danger">{{product.errors.categories}}</div>

              </div>
              
            </div>

          </div>
          
          <div class="mt-5 mb-5 ">
          
            <button type="submit" id="submitBtn" class="btn btn-primary ms-3 float-end">Submit</button>
          </div>

    </form>
    
    </div>
    <div class="col-2">
      <button  class="btn btn-warning float-end sticky-top addProdBtn" @click.stop="addProduct">Add Product</button>
    </div>

  </div>
</template>

<script>
import { MyRepositoryFactory } from "../../../../repositories/MyRepositoryFactory.js";
const ProductRepository = MyRepositoryFactory.get("products");
const BrandRepository = MyRepositoryFactory.get("brands");

export default {
  data(){
    return {
      id:0,
      form:{
        account:{
          email:"",
          password:"",
        },
        brand:{
          brandName:"",
          description:"",
        },
        prodsWithCats:[]
      },
      catForSelect:null,
      isLoading:false,
      // eslint-disable-next-line
      reg: /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/,
      errors:{
        email:null,
        password:null,
        brandName:null,
        brandDescription:null
      }
    }
  },
  methods:{
    /**loead needed data at creation of the page */
    async load(){
      this.isLoading=true
      const {data} = await ProductRepository.getDataForCreate();
      this.catForSelect = data.categories;
      this.isLoading=false

    },
    /**sends data to api if form is correct */
    async sendData(){
      if(await this.formCheck()){
        const {data} =await BrandRepository.createBrand(this.form)
        this.$router.push("/brands/" + data);
      }
    },
    /**add a product on form prodWithCatfield with default values */
    async addProduct(){
      await this.form.prodsWithCats.push({product:{name:"",shortDescription:"",description:"",price:0},
                                          categoriesIds:[],
                                          errors:{
                                            name:null,
                                            shortDescription:null,
                                            description:null,
                                            price:null,
                                            categories:null
                                          }})

      let elmnt = document.getElementById('submitBtn');
      elmnt.scrollIntoView(true);
    },
    /**removes a product from prodWithCatlist based on his index */
    removeProduct(index){
      this.form.prodsWithCats.splice(index,1)
    },
    /**check if data on form are correct
     **shows errors if there are
     **return true or false
     */
    async formCheck(){
      /** reset errors */
      this.resetBrandErrors()

      if(await this.validateEmail()){//if email is valid
        console.log("email valid")
        /**fill the prod error array if needed */

        if(!this.isPasswordValid() || !this.isBrandNameValid() || !this.isBrandDescriptionValid()   || this.checkIfProdsHaveErrors()){//if there are brand error or prod have errors
          console.log(!this.isPasswordValid() , !this.isBrandNameValid() , !this.isBrandDescriptionValid()   , this.checkIfProdsHaveErrors())
          console.log("false inside email")
          this.checkProdErrors()
          
          return false
        }else{//there are no errors here
          return true
        }

      }else{//if email is not valid

        this.checkProdErrors()

        this.isPasswordValid()
        this.isBrandNameValid()
        this.isBrandDescriptionValid()

        return false;
      }
      
      
    },
    /**check if there is a product with not valid prop, at first occurency of not valid prop returns true */
    checkIfProdsHaveErrors(){
      this.checkProdErrors()
      for(let i=0;i<this.form.prodsWithCats.length;i++){

        if(this.form.prodsWithCats[i].errors.name!=null)
          return true
        if(this.form.prodsWithCats[i].errors.shortDescription!=null)
          return true
        if(this.form.prodsWithCats[i].errors.price!=null)
          return true
        if(this.form.prodsWithCats[i].errors.categories!=null)
          return true
        }
        
        return false
    },
    /**tells if email is valid true if is valid false if not called on blur of email form input*/
    async validateEmail(){

      if(this.isEmailValid()){//if email is valid check if it already exists on database
        this.errors.email=null

       const {data} =await BrandRepository.validateEmail(this.form.account.email)
       if(!data){
         this.errors.email="Email already exists"
         return false
       }
       return data
      }
      return false
    },
    isEmailValid() {
      this.errors.email=null
      if(this.reg.test(this.form.account.email))
        return true
      this.errors.email="Not correct email format ex. example@gmail.com"
      return false;
    },
    /**method to check if password is valid add also error message */
    isPasswordValid(){
      this.errors.password=null
      if(this.form.account.password.length>0 && this.form.account.password.length<18)
        return true
      this.errors.password="Password must be between 1 and 18 characters"
      return false
    },
    /**method to check if brand name is valid also add error message */
    isBrandNameValid(){
      this.errors.brandName=null
      if(this.form.brand.brandName.length>0 && this.form.brand.brandName.length<255)
        return true
      this.errors.brandName="Brand name must be between 1 and 255 characters"
      return false
    },
    /**method to check if brand description si valid add also error message */
    isBrandDescriptionValid(){
      this.errors.brandDescription=null

      if(this.form.brand.description.length<255 )
        return true
      this.errors.brandDescription="Brand description must be lower than 255 characters"
      return false
    },
    /**method to check if product name is valid add also error message */
    isProductNameValid(product){
      
      product.errors.name=null
      if(product.product.name.length>0 && product.product.name.length<255)
        return true
      product.errors.name="Name must be bewteen 1 and 255 characters"
      return false
      
    },
    /**method to check if short description is valid add also error message */
    isProductShortDescriptionValid(product){
      product.errors.shortDescription=null
      if(product.product.shortDescription.length>0 && product.product.shortDescription.length<255)
        return true
      product.errors.shortDescription="Short description must be bewtween 1 and 255 characters"
      return false
    },
    /**method to check if price is valid adds also error message */
    isProductPriceValid(product){
      product.errors.price=null
      if(product.product.price>=0 && product.product.price<1e16)
        return true
      product.errors.price="Price can't be lower than 0 or higher than 1e16"
      return false
    },
    /**method to check if categories are valid adds also error message */
    isProductCategoriesValid(product){
      product.errors.categories=null
      if(product.categoriesIds.length>0)
        return true
      product.errors.categories="Select at least one category"
      return false
    },
    /**method that shows errors for each product on the form */
    checkProdErrors(){
      for(let i=0;i<this.form.prodsWithCats.length;i++){
        this.isProductNameValid(this.form.prodsWithCats[i])
        this.isProductShortDescriptionValid(this.form.prodsWithCats[i])
        this.isProductPriceValid(this.form.prodsWithCats[i])
        this.isProductCategoriesValid(this.form.prodsWithCats[i])
      }
    },
    /**set to null proprieties of the brand error object */
    resetBrandErrors(){
      this.errors={
        email:null,
        password:null,
        brandName:null,
        brandDescription:null

      }
    }
      
  },
  async created(){
    this.$emit("setActiveLink",2)
    await this.load()
  }

}
</script>

<style scoped>
  .bg-light{
    background-color: azure;
    border-top: 2px solid black  !important;;
  }
  .addProdBtn{
    
  position: sticky;
  top: 60px;

  }
  .err-text{
    color: rgb(84, 2, 2);
    font-size: 12px;
  }
  .bounce-enter-active {
  animation: bounce-in .5s;
  }
  .bounce-leave-active {
    animation: bounce-in .5s reverse;
  }
  .fade-enter-active, .fade-leave-active {
  transition: opacity .5s
  }
  .fade-enter, .fade-leave-to /* .fade-leave-active below version 2.1.8 */ {
    opacity: 0
  }
@keyframes bounce-in {
  0% {
    transform: scale(0);
  }
  50% {
    transform: scale(1.1);
  }
  100% {
    transform: scale(1);
  }
}
.error-input{
  border: 1px solid red !important;
}
.error-input-catbox{
  border-left: 1px solid red ;
}
</style>