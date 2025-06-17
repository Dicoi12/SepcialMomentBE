<template>
  <div class="grid">
    <div class="col-12">
      <h1 class="text-4xl font-bold mb-4">Bine ai venit!</h1>
      <p class="text-xl">Acesta este un proiect Vue.js cu TypeScript, Vue Router și PrimeVue.</p>
    </div>
    <div class="col-12 md:col-6">
      <Card>
        <template #title>
          Exemplu Card
        </template>
        <template #content>
          <p>Acesta este un exemplu de card din PrimeVue.</p>
        </template>
      </Card>
    </div>
  </div>
</template>

<script setup lang="ts">
import Card from 'primevue/card'
</script>

<style scoped>
.grid {
  padding: 2rem;
}
</style> 