<template>
    <div>
        edit brand ID:{{idBrand}}
    </div>
</template>


<script>

export default ({
    data(){
        return {
            idBrand:this.$route.params.id
        }
    }
})
</script>
