<template>
  <div class="mt-5 input-group" v-if="maxPages!=0">
        <button type="button" 
                  class=" " 
                  @click="previousPage()"
                  :class="[page==1?'disabled btn btn-outline-secondary':'btn btn-outline-primary']">Indietro</button>
        <button class="btn btn-outline-primary "
                v-for="num in pages" 
                :key="num"
                @click="changePage(num)"
                :class="[num==myPage?'active':'']"
                >{{num}}</button>
        <button type="button" 
                class="" 
                @click="nextPage()"
                :class="[myPage==maxPages?'disabled btn btn-outline-secondary':'btn btn-outline-primary']">Avanti</button>
  </div>

</template>

<script>
export default {
    data(){
        return {
            /**array that rappresents the buttons needed on the page buttons zone */
            pages:[],
            myPage:1
        }
    },
    props:{
        /**actual page */
        page:{
            type:Number,
            required:true
        },
        /**maximun number of pages */
        maxPages:{
            type:Number,
            required:true
        }
    },
    methods:{
        /**method to select the pages button needed */
        selectPages(){
            /**reset the button pages */
            this.pages=[]

            if(this.maxPages<=5){
                for(let i=1;i<=this.maxPages;i++)
                {
                    
                    this.pages.push(i)
                }
            }else{
                if(this.page<=3)
                    this.pages=[1,2,3,4,5]
                else
                    if(this.page>=this.maxPages-3) {
                        
                        this.pages=[this.maxPages-4,this.maxPages-3,this.maxPages-2,this.maxPages-1,this.maxPages]
                    }
                    else{
                        this.pages=[this.myPage-2,this.myPage-1,this.myPage,this.myPage+1,this.myPage+2]
                    }
            }

            
            

        },
        /**method to swap the actual page 
        ** @num rappresent the new page to go */
        changePage(num){
            this.myPage=num
            this.$emit("changePage",this.myPage)
            this.selectPages()
        },
        /**method that checks if there is an avaiable next page if so goes to next page else do nothing */
        async nextPage() {
            if (this.myPage < this.maxPages) {
                this.myPage = this.myPage + 1;
                this.$emit("changePage",this.myPage)
                this.selectPages()
            }

        },
        /**method that check if there is a previous page avaiable if so goes to previus page else do nothing */
        async previousPage() {

            if (this.myPage > 1) {
                this.myPage = this.myPage - 1;
                this.$emit("changePage",this.myPage)
                this.selectPages()

            }

        },
        updatePage(){
            this.myPage=this.page
        }
    },
    watch:{
        maxPages(){
            this.selectPages()
        },
        page(){
            this.myPage=this.page
            this.selectPages()
            
        }

    },
    /**on mount of the component load the paging buttons needed */
    mounted(){
        this.selectPages()
        this.updatePage()
    }
}
</script>

<style>

</style>