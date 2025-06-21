<template>
  <div class="events-container">
    <!-- Header Section -->
    <div class="events-header">
      <div class="header-content">
        <h1 class="page-title">Evenimentele Mele</h1>
        <p class="page-subtitle">Gestionează și planifică evenimentele tale speciale</p>
      </div>
      <Button 
        @click="showCreateModal = true" 
        label="Eveniment Nou" 
        icon="pi pi-plus"
        severity="primary" 
        size="large"
        class="create-btn"
      />
    </div>

    <!-- Filters Section -->
    <div class="filters-section">
      <div class="search-box">
        <i class="pi pi-search search-icon"></i>
        <InputText
          v-model="searchQuery"
          placeholder="Caută evenimente..."
          class="search-input"
        />
      </div>
      
      <div class="filter-options">
        <Dropdown
          v-model="selectedEventType"
          :options="eventTypeOptions"
          optionLabel="label"
          optionValue="value"
          placeholder="Tip Eveniment"
          class="filter-dropdown"
        />
        
        <Calendar
          v-model="selectedDate"
          placeholder="Data Eveniment"
          class="filter-calendar"
          :showIcon="true"
        />
        
        <Button 
          @click="clearFilters" 
          label="Resetează" 
          severity="secondary" 
          size="small"
          class="reset-btn"
        />
      </div>
    </div>

    <!-- Loading State -->
    <div v-if="isLoading" class="loading-container">
      <ProgressSpinner size="large" />
      <p>Se încarcă evenimentele...</p>
    </div>

    <!-- Events Grid -->
    <div v-else-if="filteredEvents.length > 0" class="events-grid">
      <Card 
        v-for="event in filteredEvents" 
        :key="event.id" 
        class="event-card"
        :class="{ 'upcoming': isUpcoming(event.date) }"
      >
        <template #header>
          <div class="event-header">
            <div class="event-type-badge" :class="getEventTypeClass(event.eventType)">
              <i :class="getEventTypeIcon(event.eventType)"></i>
              {{ getEventTypeLabel(event.eventType) }}
            </div>
            <div class="event-actions">
              <Button 
                @click="editEvent(event)" 
                icon="pi pi-pencil" 
                severity="secondary" 
                size="small"
                text
                rounded
              />
              <Button 
                @click="deleteEvent(event.id)" 
                icon="pi pi-trash" 
                severity="danger" 
                size="small"
                text
                rounded
              />
            </div>
          </div>
        </template>
        
        <template #title>
          <h3 class="event-title">{{ event.title }}</h3>
        </template>
        
        <template #content>
          <div class="event-details">
            <div class="event-info">
              <div class="info-item">
                <i class="pi pi-calendar"></i>
                <span>{{ formatDate(event.date) }}</span>
              </div>
              <div class="info-item">
                <i class="pi pi-map-marker"></i>
                <span>{{ event.location }}</span>
              </div>
              <div class="info-item">
                <i class="pi pi-clock"></i>
                <span>{{ formatTime(event.date) }}</span>
              </div>
            </div>
            
            <p class="event-description">{{ event.description }}</p>
            
            <div class="event-stats">
              <div class="stat-item">
                <i class="pi pi-users"></i>
                <span>{{ event.weddingGuests?.length || 0 }} invitați</span>
              </div>
              <div class="stat-item">
                <i class="pi pi-table"></i>
                <span>{{ event.tables?.length || 0 }} mese</span>
              </div>
              <div class="stat-item">
                <i class="pi pi-dollar"></i>
                <span>{{ event.weddingExpenses?.length || 0 }} cheltuieli</span>
              </div>
            </div>
          </div>
        </template>
        
        <template #footer>
          <div class="event-footer">
            <Button 
              @click="viewEventDetails(event)" 
              label="Vezi Detalii" 
              severity="primary" 
              size="small"
              outlined
            />
            <Button 
              @click="manageEvent(event)" 
              label="Gestionează" 
              severity="secondary" 
              size="small"
              outlined
            />
          </div>
        </template>
      </Card>
    </div>

    <!-- Empty State -->
    <div v-else class="empty-state">
      <i class="pi pi-calendar empty-icon"></i>
      <h3>Nu ai evenimente încă</h3>
      <p>Creează primul tău eveniment pentru a începe să planifici momentele tale speciale</p>
      <Button 
        @click="showCreateModal = true" 
        label="Creează Eveniment" 
        icon="pi pi-plus"
        severity="primary" 
        size="large"
      />
    </div>

    <!-- Create/Edit Event Modal -->
    <Dialog 
      v-model:visible="showCreateModal" 
      :header="isEditing ? 'Editează Eveniment' : 'Eveniment Nou'"
      modal 
      class="event-modal"
      :style="{ width: '600px' }"
    >
      <form @submit.prevent="saveEvent" class="event-form">
        <div class="form-group">
          <label for="title">Titlu Eveniment</label>
          <InputText
            id="title"
            v-model="eventForm.title"
            placeholder="Ex: Nunta Maria și Ion"
            class="form-input"
            :class="{ 'p-invalid': errors.title }"
          />
          <small v-if="errors.title" class="error-message">{{ errors.title }}</small>
        </div>

        <div class="form-group">
          <label for="eventType">Tip Eveniment</label>
          <Dropdown
            id="eventType"
            v-model="eventForm.eventType"
            :options="eventTypeOptions"
            optionLabel="label"
            optionValue="value"
            placeholder="Selectează tipul"
            class="form-input"
            :class="{ 'p-invalid': errors.eventType }"
          />
          <small v-if="errors.eventType" class="error-message">{{ errors.eventType }}</small>
        </div>

        <div class="form-row">
          <div class="form-group">
            <label for="date">Data Eveniment</label>
            <Calendar
              id="date"
              v-model="eventForm.date"
              :showIcon="true"
              class="form-input"
              :class="{ 'p-invalid': errors.date }"
            />
            <small v-if="errors.date" class="error-message">{{ errors.date }}</small>
          </div>

          <div class="form-group">
            <label for="time">Ora Eveniment</label>
            <Calendar
              id="time"
              v-model="eventForm.date"
              timeOnly
              hourFormat="24"
              class="form-input"
            />
          </div>
        </div>

        <div class="form-group">
          <label for="location">Locație</label>
          <InputText
            id="location"
            v-model="eventForm.location"
            placeholder="Ex: Restaurantul La Strada"
            class="form-input"
            :class="{ 'p-invalid': errors.location }"
          />
          <small v-if="errors.location" class="error-message">{{ errors.location }}</small>
        </div>

        <div class="form-group">
          <label for="description">Descriere</label>
          <Textarea
            id="description"
            v-model="eventForm.description"
            placeholder="Descrie evenimentul tău..."
            rows="4"
            class="form-input"
            :class="{ 'p-invalid': errors.description }"
          />
          <small v-if="errors.description" class="error-message">{{ errors.description }}</small>
        </div>
      </form>

      <template #footer>
        <Button 
          @click="showCreateModal = false" 
          label="Anulează" 
          severity="secondary" 
          outlined
        />
        <Button 
          @click="saveEvent" 
          :label="isEditing ? 'Actualizează' : 'Creează'" 
          severity="primary"
          :loading="isSaving"
        />
      </template>
    </Dialog>

    <!-- Delete Confirmation Modal -->
    <Dialog 
      v-model:visible="showDeleteModal" 
      header="Confirmă Ștergerea"
      modal 
      class="delete-modal"
      :style="{ width: '400px' }"
    >
      <div class="delete-content">
        <i class="pi pi-exclamation-triangle delete-icon"></i>
        <p>Ești sigur că vrei să ștergi evenimentul "{{ eventToDelete?.title }}"?</p>
        <p class="delete-warning">Această acțiune nu poate fi anulată.</p>
      </div>

      <template #footer>
        <Button 
          @click="showDeleteModal = false" 
          label="Anulează" 
          severity="secondary" 
          outlined
        />
        <Button 
          @click="confirmDelete" 
          label="Șterge" 
          severity="danger"
          :loading="isDeleting"
        />
      </template>
    </Dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useUserStore } from '../stores/user'

const router = useRouter()
const userStore = useUserStore()

// State
const events = ref<any[]>([])
const isLoading = ref(false)
const isSaving = ref(false)
const isDeleting = ref(false)
const showCreateModal = ref(false)
const showDeleteModal = ref(false)
const isEditing = ref(false)
const eventToDelete = ref<any>(null)

// Filters
const searchQuery = ref('')
const selectedEventType = ref('')
const selectedDate = ref<Date | null>(null)

// Form
const eventForm = ref({
  id: 0,
  title: '',
  eventType: '',
  date: new Date(),
  location: '',
  description: ''
})

const errors = ref({
  title: '',
  eventType: '',
  date: '',
  location: '',
  description: ''
})

// Event Type Options
const eventTypeOptions = [
  { label: 'Nuntă', value: 'Wedding' },
  { label: 'Botez', value: 'Baptism' },
  { label: 'Aniversare', value: 'Anniversary' }
]

// Computed
const filteredEvents = computed(() => {
  let filtered = events.value

  if (searchQuery.value) {
    filtered = filtered.filter(event => 
      event.title.toLowerCase().includes(searchQuery.value.toLowerCase()) ||
      event.description.toLowerCase().includes(searchQuery.value.toLowerCase()) ||
      event.location.toLowerCase().includes(searchQuery.value.toLowerCase())
    )
  }

  if (selectedEventType.value) {
    filtered = filtered.filter(event => event.eventType === selectedEventType.value)
  }

  if (selectedDate.value) {
    const selectedDateStr = selectedDate.value.toDateString()
    filtered = filtered.filter(event => 
      new Date(event.date).toDateString() === selectedDateStr
    )
  }

  return filtered.sort((a, b) => new Date(a.date).getTime() - new Date(b.date).getTime())
})

// Methods
const loadEvents = async () => {
  isLoading.value = true
  try {
    const response = await fetch('/api/Event', {
      headers: {
        'Authorization': `Bearer ${userStore.token}`
      }
    })
    
    if (response.ok) {
      events.value = await response.json()
    } else {
      console.error('Eroare la încărcarea evenimentelor')
    }
  } catch (error) {
    console.error('Eroare la încărcarea evenimentelor:', error)
  } finally {
    isLoading.value = false
  }
}

const saveEvent = async () => {
  if (!validateForm()) return

  isSaving.value = true
  try {
    const url = isEditing.value 
      ? `/api/Event/${eventForm.value.id}`
      : '/api/Event'
    
    const method = isEditing.value ? 'PUT' : 'POST'
    
    const response = await fetch(url, {
      method,
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${userStore.token}`
      },
      body: JSON.stringify({
        ...eventForm.value,
        userId: userStore.user?.id
      })
    })

    if (response.ok) {
      showCreateModal.value = false
      resetForm()
      await loadEvents()
    } else {
      console.error('Eroare la salvarea evenimentului')
    }
  } catch (error) {
    console.error('Eroare la salvarea evenimentului:', error)
  } finally {
    isSaving.value = false
  }
}

const deleteEvent = (eventId: number) => {
  const event = events.value.find(e => e.id === eventId)
  eventToDelete.value = event
  showDeleteModal.value = true
}

const confirmDelete = async () => {
  if (!eventToDelete.value) return

  isDeleting.value = true
  try {
    const response = await fetch(`/api/Event/${eventToDelete.value.id}`, {
      method: 'DELETE',
      headers: {
        'Authorization': `Bearer ${userStore.token}`
      }
    })

    if (response.ok) {
      showDeleteModal.value = false
      eventToDelete.value = null
      await loadEvents()
    } else {
      console.error('Eroare la ștergerea evenimentului')
    }
  } catch (error) {
    console.error('Eroare la ștergerea evenimentului:', error)
  } finally {
    isDeleting.value = false
  }
}

const editEvent = (event: any) => {
  eventForm.value = { ...event }
  isEditing.value = true
  showCreateModal.value = true
}

const viewEventDetails = (event: any) => {
  router.push(`/events/${event.id}`)
}

const manageEvent = (event: any) => {
  router.push(`/events/${event.id}/manage`)
}

const clearFilters = () => {
  searchQuery.value = ''
  selectedEventType.value = ''
  selectedDate.value = null
}

const resetForm = () => {
  eventForm.value = {
    id: 0,
    title: '',
    eventType: '',
    date: new Date(),
    location: '',
    description: ''
  }
  errors.value = {
    title: '',
    eventType: '',
    date: '',
    location: '',
    description: ''
  }
  isEditing.value = false
}

const validateForm = () => {
  errors.value = {
    title: '',
    eventType: '',
    date: '',
    location: '',
    description: ''
  }

  let isValid = true

  if (!eventForm.value.title.trim()) {
    errors.value.title = 'Titlul este obligatoriu'
    isValid = false
  }

  if (!eventForm.value.eventType) {
    errors.value.eventType = 'Tipul evenimentului este obligatoriu'
    isValid = false
  }

  if (!eventForm.value.date) {
    errors.value.date = 'Data evenimentului este obligatorie'
    isValid = false
  }

  if (!eventForm.value.location.trim()) {
    errors.value.location = 'Locația este obligatorie'
    isValid = false
  }

  if (!eventForm.value.description.trim()) {
    errors.value.description = 'Descrierea este obligatorie'
    isValid = false
  }

  return isValid
}

// Utility functions
const formatDate = (date: string | Date) => {
  return new Date(date).toLocaleDateString('ro-RO', {
    year: 'numeric',
    month: 'long',
    day: 'numeric'
  })
}

const formatTime = (date: string | Date) => {
  return new Date(date).toLocaleTimeString('ro-RO', {
    hour: '2-digit',
    minute: '2-digit'
  })
}

const isUpcoming = (date: string | Date) => {
  return new Date(date) > new Date()
}

const getEventTypeLabel = (type: string) => {
  const option = eventTypeOptions.find(opt => opt.value === type)
  return option ? option.label : type
}

const getEventTypeIcon = (type: string) => {
  switch (type) {
    case 'Wedding': return 'pi pi-heart'
    case 'Baptism': return 'pi pi-star'
    case 'Anniversary': return 'pi pi-calendar-times'
    default: return 'pi pi-calendar'
  }
}

const getEventTypeClass = (type: string) => {
  switch (type) {
    case 'Wedding': return 'wedding'
    case 'Baptism': return 'baptism'
    case 'Anniversary': return 'anniversary'
    default: return 'default'
  }
}

// Lifecycle
onMounted(() => {
  loadEvents()
})
</script>

<style scoped>
.events-container {
  min-height: 100vh;
  background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
  padding: 2rem;
}

.events-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 2rem;
  background: white;
  padding: 2rem;
  border-radius: 20px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
}

.header-content {
  flex: 1;
}

.page-title {
  font-size: 2.5rem;
  font-weight: 700;
  color: #2c3e50;
  margin: 0 0 0.5rem 0;
  font-family: 'Julee', cursive;
  background: linear-gradient(135deg, #c44569, #ff6b9d);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent;
  background-clip: text;
}

.page-subtitle {
  color: #7f8c8d;
  font-size: 1.1rem;
  margin: 0;
}

.create-btn {
  padding: 1rem 2rem;
  font-weight: 600;
  border-radius: 15px;
  background: linear-gradient(135deg, #c44569, #ff6b9d);
  border: none;
  box-shadow: 0 8px 20px rgba(196, 69, 105, 0.3);
}

.create-btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 12px 25px rgba(196, 69, 105, 0.4);
}

.filters-section {
  background: white;
  padding: 1.5rem;
  border-radius: 15px;
  margin-bottom: 2rem;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
}

.search-box {
  position: relative;
  margin-bottom: 1rem;
}

.search-icon {
  position: absolute;
  left: 15px;
  top: 50%;
  transform: translateY(-50%);
  color: #c44569;
  z-index: 1;
}

.search-input {
  width: 100%;
  padding: 12px 15px 12px 45px;
  border: 2px solid #f8b5d3;
  border-radius: 12px;
  font-size: 1rem;
  transition: all 0.3s ease;
}

.search-input:focus {
  border-color: #c44569;
  box-shadow: 0 0 0 4px rgba(196, 69, 105, 0.1);
}

.filter-options {
  display: flex;
  gap: 1rem;
  align-items: center;
  flex-wrap: wrap;
}

.filter-dropdown,
.filter-calendar {
  min-width: 200px;
}

.reset-btn {
  padding: 8px 16px;
  border-radius: 8px;
}

.loading-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 4rem;
  color: #7f8c8d;
}

.events-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(400px, 1fr));
  gap: 2rem;
}

.event-card {
  transition: all 0.3s ease;
  border-radius: 15px;
  overflow: hidden;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
}

.event-card:hover {
  transform: translateY(-5px);
  box-shadow: 0 8px 30px rgba(0, 0, 0, 0.15);
}

.event-card.upcoming {
  border-left: 4px solid #27ae60;
}

.event-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem;
}

.event-type-badge {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.5rem 1rem;
  border-radius: 20px;
  font-size: 0.9rem;
  font-weight: 600;
  color: white;
}

.event-type-badge.wedding {
  background: linear-gradient(135deg, #e74c3c, #c0392b);
}

.event-type-badge.baptism {
  background: linear-gradient(135deg, #3498db, #2980b9);
}

.event-type-badge.anniversary {
  background: linear-gradient(135deg, #9b59b6, #8e44ad);
}

.event-type-badge.default {
  background: linear-gradient(135deg, #95a5a6, #7f8c8d);
}

.event-actions {
  display: flex;
  gap: 0.5rem;
}

.event-title {
  font-size: 1.4rem;
  font-weight: 700;
  color: #2c3e50;
  margin: 0 0 1rem 0;
}

.event-details {
  padding: 1rem 0;
}

.event-info {
  margin-bottom: 1rem;
}

.info-item {
  display: flex;
  align-items: center;
  gap: 0.8rem;
  margin-bottom: 0.5rem;
  color: #7f8c8d;
}

.info-item i {
  color: #c44569;
  width: 16px;
}

.event-description {
  color: #7f8c8d;
  line-height: 1.6;
  margin-bottom: 1rem;
}

.event-stats {
  display: flex;
  gap: 1.5rem;
  padding-top: 1rem;
  border-top: 1px solid #ecf0f1;
}

.stat-item {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  color: #7f8c8d;
  font-size: 0.9rem;
}

.stat-item i {
  color: #c44569;
}

.event-footer {
  display: flex;
  gap: 1rem;
  justify-content: flex-end;
}

.empty-state {
  text-align: center;
  padding: 4rem 2rem;
  color: #7f8c8d;
}

.empty-icon {
  font-size: 4rem;
  color: #c44569;
  margin-bottom: 1rem;
}

.empty-state h3 {
  font-size: 1.8rem;
  color: #2c3e50;
  margin-bottom: 1rem;
}

.empty-state p {
  font-size: 1.1rem;
  margin-bottom: 2rem;
  max-width: 400px;
  margin-left: auto;
  margin-right: auto;
}

.event-form {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.form-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1rem;
}

.form-group label {
  font-weight: 600;
  color: #2c3e50;
}

.form-input {
  padding: 12px;
  border: 2px solid #f8b5d3;
  border-radius: 8px;
  font-size: 1rem;
  transition: all 0.3s ease;
}

.form-input:focus {
  border-color: #c44569;
  box-shadow: 0 0 0 4px rgba(196, 69, 105, 0.1);
}

.form-input.p-invalid {
  border-color: #e74c3c;
}

.error-message {
  color: #e74c3c;
  font-size: 0.85rem;
  font-weight: 500;
}

.delete-content {
  text-align: center;
  padding: 1rem;
}

.delete-icon {
  font-size: 3rem;
  color: #e74c3c;
  margin-bottom: 1rem;
}

.delete-warning {
  color: #e74c3c;
  font-weight: 600;
  margin-top: 0.5rem;
}

/* Responsive Design */
@media (max-width: 1024px) {
  .events-container {
    padding: 1.5rem;
  }
  
  .events-header {
    flex-direction: column;
    gap: 1.5rem;
    text-align: center;
    align-items: stretch;
  }
  
  .page-title {
    font-size: 2rem;
  }
  
  .events-grid {
    grid-template-columns: repeat(auto-fill, minmax(350px, 1fr));
    gap: 1.5rem;
  }
  
  .create-btn {
    align-self: flex-end;
    width: 50px;
    height: 50px;
    border-radius: 50%;
    padding: 0;
  }

  .create-btn :deep(.p-button-label) {
    display: none;
  }
  
  .filter-options {
    flex-direction: column;
    align-items: stretch;
    gap: 1rem;
  }
  
  .filter-dropdown,
  .filter-calendar,
  .reset-btn {
    min-width: auto;
    width: 100%;
  }
  
  .events-grid {
    grid-template-columns: 1fr;
    gap: 1rem;
  }
  
  .form-row {
    grid-template-columns: 1fr;
  }
  
  .event-stats {
    flex-direction: column;
    gap: 0.5rem;
  }
  
  .event-footer {
    flex-direction: column;
  }
}

@media (max-width: 768px) {
  .events-container {
    padding: 1rem;
  }
  
  .events-header {
    padding: 1.5rem;
  }
  
  .page-title {
    font-size: 1.8rem;
  }
  
  .filter-options {
    flex-direction: column;
    align-items: stretch;
  }
  
  .filter-dropdown,
  .filter-calendar {
    min-width: auto;
  }
  
  .events-grid {
    grid-template-columns: 1fr;
    gap: 1rem;
  }
  
  .form-row {
    grid-template-columns: 1fr;
  }
  
  .event-stats {
    flex-direction: column;
    gap: 0.5rem;
  }
  
  .event-footer {
    flex-direction: column;
  }
}

@media (max-width: 480px) {
  .events-header {
    padding: 1rem;
  }
  
  .page-title {
    font-size: 1.5rem;
  }
  
  .create-btn {
    padding: 0.8rem 1.5rem;
    font-size: 0.9rem;
  }
  
  .event-card {
    margin: 0 0.5rem;
  }
}
</style>
