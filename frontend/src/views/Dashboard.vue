<template>
  <div class="dashboard-container">
    <div class="dashboard-header">
      <h1>Dashboard - SpecialMoment</h1>
      <p>Bine ai venit, {{ userStore.userName }}!</p>
      <Button @click="userStore.logout()" label="Deconectare" severity="danger" />
    </div>
    
    <div class="dashboard-content">
      <div class="stats-grid">
        <Card class="stat-card">
          <template #title>
            <i class="pi pi-calendar"></i>
            Evenimente Active
          </template>
          <template #content>
            <div class="stat-number">12</div>
            <p>Evenimente în curs de planificare</p>
          </template>
        </Card>
        
        <Card class="stat-card">
          <template #title>
            <i class="pi pi-check-circle"></i>
            Evenimente Finalizate
          </template>
          <template #content>
            <div class="stat-number">8</div>
            <p>Evenimente planificate cu succes</p>
          </template>
        </Card>
        
        <Card class="stat-card">
          <template #title>
            <i class="pi pi-users"></i>
            Invitați Total
          </template>
          <template #content>
            <div class="stat-number">156</div>
            <p>Invitați în toate evenimentele</p>
          </template>
        </Card>
      </div>
      
      <div class="recent-events">
        <h2>Evenimente Recente</h2>
        <div class="events-list">
          <Card v-for="event in recentEvents" :key="event.id" class="event-card">
            <template #title>
              {{ event.title }}
            </template>
            <template #content>
              <div class="event-details">
                <p><i class="pi pi-calendar"></i> {{ event.date }}</p>
                <p><i class="pi pi-map-marker"></i> {{ event.location }}</p>
                <p><i class="pi pi-users"></i> {{ event.guests }} invitați</p>
              </div>
              <div class="event-actions">
                <Button label="Vezi Detalii" severity="primary" size="small" />
                <Button label="Editează" severity="secondary" size="small" />
              </div>
            </template>
          </Card>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
// Importurile sunt automatizate de unplugin-auto-import și unplugin-vue-components
const userStore = useUserStore()

const recentEvents = [
  {
    id: 1,
    title: 'Nunta Ana & Mihai',
    date: '15 Iulie 2024',
    location: 'București, România',
    guests: 120
  },
  {
    id: 2,
    title: 'Ziua de Naștere Maria',
    date: '22 Iunie 2024',
    location: 'Cluj-Napoca, România',
    guests: 25
  },
  {
    id: 3,
    title: 'Botezul Alex',
    date: '10 August 2024',
    location: 'Timișoara, România',
    guests: 45
  }
]
</script>

<style scoped>
.dashboard-container {
  min-height: 100vh;
  background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%);
  padding: 2rem;
}

.dashboard-header {
  text-align: center;
  margin-bottom: 3rem;
  color: #2c3e50;
}

.dashboard-header h1 {
  font-size: 2.5rem;
  margin-bottom: 0.5rem;
  font-family: 'Julee', cursive;
}

.dashboard-header p {
  font-size: 1.2rem;
  margin-bottom: 1rem;
  color: #7f8c8d;
}

.stats-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: 2rem;
  margin-bottom: 3rem;
}

.stat-card {
  text-align: center;
  transition: transform 0.3s ease;
}

.stat-card:hover {
  transform: translateY(-5px);
}

.stat-card :deep(.p-card-title) {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
  color: #667eea;
  font-weight: 600;
}

.stat-number {
  font-size: 3rem;
  font-weight: 700;
  color: #667eea;
  margin-bottom: 0.5rem;
}

.recent-events h2 {
  color: #2c3e50;
  margin-bottom: 1.5rem;
  font-size: 1.8rem;
}

.events-list {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(350px, 1fr));
  gap: 1.5rem;
}

.event-card {
  transition: transform 0.3s ease;
}

.event-card:hover {
  transform: translateY(-3px);
}

.event-details {
  margin-bottom: 1rem;
}

.event-details p {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  margin-bottom: 0.5rem;
  color: #7f8c8d;
}

.event-details i {
  color: #667eea;
  width: 16px;
}

.event-actions {
  display: flex;
  gap: 0.5rem;
  justify-content: flex-end;
}

@media (max-width: 768px) {
  .dashboard-container {
    padding: 1rem;
  }
  
  .dashboard-header h1 {
    font-size: 2rem;
  }
  
  .stats-grid {
    grid-template-columns: 1fr;
  }
  
  .events-list {
    grid-template-columns: 1fr;
  }
}
</style> 