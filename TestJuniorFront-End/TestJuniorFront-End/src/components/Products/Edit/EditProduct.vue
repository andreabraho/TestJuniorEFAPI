<template>
    <div>
        edit prod ID:{{idProduct}}
    </div>
</template>

<script>

export default({
    data(){
        return{
            idProduct:this.$route.params.id
        }
    }
})
</script>
