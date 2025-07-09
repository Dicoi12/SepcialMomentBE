<template>
  <div class="events-container">
    <!-- Header Section -->
    <div class="events-header">
      <div class="header-content">
        <h1 class="page-title">Evenimentele Mele</h1>
        <p class="page-subtitle">Gestionează și planifică evenimentele tale speciale</p>
      </div>
      <Button 
        @click="startEventWizard" 
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
        @click="startEventWizard" 
        label="Creează Eveniment" 
        icon="pi pi-plus"
        severity="primary" 
        size="large"
      />
    </div>

    <!-- Event Wizard Modal -->
    <Dialog 
      v-model:visible="showWizardModal" 
      :header="wizardHeader"
      modal 
      class="wizard-modal"
      :style="{ width: '800px', maxWidth: '90vw' }"
      :closable="false"
    >
      <!-- Wizard Progress -->
      <div class="wizard-progress">
        <div class="progress-steps">
          <div 
            v-for="(step, index) in wizardSteps" 
            :key="index"
            class="progress-step"
            :class="{ 
              'active': currentStep === index,
              'completed': currentStep > index,
              'disabled': currentStep < index
            }"
          >
            <div class="step-number">{{ index + 1 }}</div>
            <div class="step-label">{{ step.label }}</div>
          </div>
        </div>
        <div class="progress-bar">
          <div 
            class="progress-fill" 
            :style="{ width: `${(currentStep / (wizardSteps.length - 1)) * 100}%` }"
          ></div>
        </div>
      </div>

      <!-- Step 1: Basic Event Information -->
      <div v-if="currentStep === 0" class="wizard-step">
        <h3>Informații de Bază</h3>
        <p class="step-description">Completează informațiile principale despre eveniment</p>
        
        <form @submit.prevent="nextStep" class="wizard-form">
          <div class="form-group">
            <label for="title">Titlu Eveniment *</label>
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
            <label for="eventType">Tip Eveniment *</label>
            <Dropdown
              id="eventType"
              v-model="eventForm.eventType"
              :options="eventTypeOptions"
              optionLabel="label"
              optionValue="value"
              placeholder="Selectează tipul"
              class="form-input"
              :class="{ 'p-invalid': errors.eventType }"
              @change="onEventTypeChange"
            />
            <small v-if="errors.eventType" class="error-message">{{ errors.eventType }}</small>
          </div>

          <div class="form-row">
            <div class="form-group">
              <label for="date">Data Eveniment *</label>
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
            <label for="location">Locație *</label>
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
            <label for="description">Descriere *</label>
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
      </div>

      <!-- Step 2: Event Form Template -->
      <div v-if="currentStep === 1" class="wizard-step">
        <h3>Configurare Eveniment</h3>
        <p class="step-description">Completează detaliile specifice pentru tipul de eveniment selectat</p>
        
        <div v-if="isLoadingTemplate" class="template-loading">
          <ProgressSpinner size="large" />
          <p>Se încarcă template-ul pentru {{ getEventTypeLabel(eventForm.eventType) }}...</p>
        </div>
        
        <div v-else-if="eventFormTemplate" class="template-form">
          <div 
            v-for="field in eventFormTemplate.fields" 
            :key="field.fieldName"
            class="form-group"
          >
            <label :for="field.fieldName">{{ field.fieldLabel }} {{ field.isRequired ? '*' : '' }}</label>
            
            <!-- Input Text -->
            <InputText
              v-if="field.fieldType === 'text'"
              :id="field.fieldName"
              v-model="templateFormData[field.fieldName]"
              :placeholder="field.helpText"
              class="form-input"
              :class="{ 'p-invalid': templateErrors[field.fieldName] }"
            />
            
            <!-- Textarea -->
            <Textarea
              v-else-if="field.fieldType === 'textarea'"
              :id="field.fieldName"
              v-model="templateFormData[field.fieldName]"
              :placeholder="field.helpText"
              :rows="4"
              class="form-input"
              :class="{ 'p-invalid': templateErrors[field.fieldName] }"
            />
            
            <!-- Number Input -->
            <InputNumber
              v-else-if="field.fieldType === 'number'"
              :id="field.fieldName"
              v-model="templateFormData[field.fieldName]"
              :placeholder="field.helpText"
              class="form-input"
              :class="{ 'p-invalid': templateErrors[field.fieldName] }"
            />
            
            <!-- Dropdown/Select -->
            <Dropdown
              v-else-if="field.fieldType === 'select'"
              :id="field.fieldName"
              v-model="templateFormData[field.fieldName]"
              :options="field.options ? parseOptions(field.options) : []"
              optionLabel="label"
              optionValue="value"
              :placeholder="field.helpText"
              class="form-input"
              :class="{ 'p-invalid': templateErrors[field.fieldName] }"
            />
            
            <!-- Checkbox -->
            <div v-else-if="field.fieldType === 'checkbox'" class="checkbox-group">
              <Checkbox
                :id="field.fieldName"
                v-model="templateFormData[field.fieldName]"
                :binary="true"
              />
              <label :for="field.fieldName" class="checkbox-label">{{ field.fieldLabel }}</label>
            </div>
            
            <!-- Date Picker -->
            <Calendar
              v-else-if="field.fieldType === 'date'"
              :id="field.fieldName"
              v-model="templateFormData[field.fieldName]"
              :showIcon="true"
              :placeholder="field.helpText"
              class="form-input"
              :class="{ 'p-invalid': templateErrors[field.fieldName] }"
            />
            
            <!-- Time Picker -->
            <Calendar
              v-else-if="field.fieldType === 'time'"
              :id="field.fieldName"
              v-model="templateFormData[field.fieldName]"
              timeOnly
              hourFormat="24"
              :placeholder="field.helpText"
              class="form-input"
              :class="{ 'p-invalid': templateErrors[field.fieldName] }"
            />
            
            <small v-if="templateErrors[field.fieldName]" class="error-message">{{ templateErrors[field.fieldName] }}</small>
            <small v-if="field.helpText" class="help-text">{{ field.helpText }}</small>
          </div>
        </div>
        
        <div v-else class="no-template">
          <i class="pi pi-info-circle"></i>
          <p>Nu există template specific pentru acest tip de eveniment. Poți continua cu informațiile de bază.</p>
        </div>
      </div>

      <!-- Step 3: Review and Save -->
      <div v-if="currentStep === 2" class="wizard-step">
        <h3>Revizuire și Salvare</h3>
        <p class="step-description">Verifică informațiile și salvează evenimentul</p>
        
        <div class="review-section">
          <h4>Informații de Bază</h4>
          <div class="review-item">
            <strong>Titlu:</strong> {{ eventForm.title }}
          </div>
          <div class="review-item">
            <strong>Tip:</strong> {{ getEventTypeLabel(eventForm.eventType) }}
          </div>
          <div class="review-item">
            <strong>Data și Ora:</strong> {{ formatDateTime(eventForm.date) }}
          </div>
          <div class="review-item">
            <strong>Locație:</strong> {{ eventForm.location }}
          </div>
          <div class="review-item">
            <strong>Descriere:</strong> {{ eventForm.description }}
          </div>
          
          <div v-if="Object.keys(templateFormData).length > 0" class="template-review">
            <h4>Configurare Specifică</h4>
            <div 
              v-for="(value, key) in templateFormData" 
              :key="key"
              class="review-item"
            >
              <strong>{{ getFieldLabel(key) }}:</strong> {{ formatFieldValue(value) }}
            </div>
          </div>
        </div>
      </div>

      <!-- Wizard Footer -->
      <template #footer>
        <div class="wizard-footer">
          <Button 
            v-if="currentStep > 0"
            @click="previousStep" 
            label="Înapoi" 
            severity="secondary" 
            outlined
          />
          <Button 
            v-if="currentStep < wizardSteps.length - 1"
            @click="nextStep" 
            :label="currentStep === 1 ? 'Revizuire' : 'Următorul Pas'"
            severity="primary"
            :loading="isLoadingTemplate"
          />
          <Button 
            v-if="currentStep === wizardSteps.length - 1"
            @click="saveEvent" 
            label="Salvează Eveniment" 
            severity="success"
            :loading="isSaving"
          />
          <Button 
            @click="closeWizard" 
            label="Anulează" 
            severity="secondary" 
            outlined
          />
        </div>
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
import type { Event, EventFormTemplate, CreateEventRequest } from '../types/event'
import fetchApi from '../fetch'

const router = useRouter()
const userStore = useUserStore()

// State
const events = ref<Event[]>([])
const isLoading = ref(false)
const isSaving = ref(false)
const isDeleting = ref(false)
const showWizardModal = ref(false)
const showDeleteModal = ref(false)
const eventToDelete = ref<Event | null>(null)

// Wizard State
const currentStep = ref(0)
const isLoadingTemplate = ref(false)
const eventFormTemplate = ref<EventFormTemplate | null>(null)
const templateFormData = ref<Record<string, any>>({})
const templateErrors = ref<Record<string, string>>({})

// Filters
const searchQuery = ref('')
const selectedEventType = ref('')
const selectedDate = ref<Date | null>(null)

// Form
const eventForm = ref<CreateEventRequest>({
  title: '',
  eventType: '',
  date: new Date(),
  location: '',
  description: '',
  userId: Number(userStore.user?.id) || 0
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
  { label: 'Nuntă', value: 'nunta' },
  { label: 'Botez', value: 'Baptism' },
  { label: 'Aniversare', value: 'Anniversary' }
]

// Wizard Steps
const wizardSteps = [
  { label: 'Informații de Bază', icon: 'pi pi-info-circle' },
  { label: 'Configurare Specifică', icon: 'pi pi-cog' },
  { label: 'Revizuire', icon: 'pi pi-check-circle' }
]

// Computed
const wizardHeader = computed(() => {
  return `Creează Eveniment - ${wizardSteps[currentStep.value].label}`
})

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
const startEventWizard = () => {
  resetWizard()
  showWizardModal.value = true
}

const resetWizard = () => {
  currentStep.value = 0
  eventForm.value = {
    title: '',
    eventType: '',
    date: new Date(),
    location: '',
    description: '',
    userId: Number(userStore.user?.id) || 0
  }
  errors.value = {
    title: '',
    eventType: '',
    date: '',
    location: '',
    description: ''
  }
  eventFormTemplate.value = null
  templateFormData.value = {}
  templateErrors.value = {}
}

const closeWizard = () => {
  showWizardModal.value = false
  resetWizard()
}

const nextStep = async () => {
  if (currentStep.value === 0) {
    if (!validateBasicForm()) return
    await loadEventFormTemplate()
  } else if (currentStep.value === 1) {
    if (!validateTemplateForm()) return
  }
  
  if (currentStep.value < wizardSteps.length - 1) {
    currentStep.value++
  }
}

const previousStep = () => {
  if (currentStep.value > 0) {
    currentStep.value--
  }
}

const onEventTypeChange = () => {
  // Reset template data when event type changes
  eventFormTemplate.value = null
  templateFormData.value = {}
  templateErrors.value = {}
}

const loadEventFormTemplate = async () => {
  if (!eventForm.value.eventType) return
  
  isLoadingTemplate.value = true
  try {
    const response = await fetchApi(`EventFormTemplate/by-event-type/${eventForm.value.eventType}`, 'GET')
    if (response.status === 200) {
      // Sortăm câmpurile după displayOrder
      const fields = response.data.sort((a: any, b: any) => a.displayOrder - b.displayOrder)
      console.log(fields)
      eventFormTemplate.value = {
        id: 1,
        eventType: eventForm.value.eventType,
        name: `Template pentru ${getEventTypeLabel(eventForm.value.eventType)}`,
        description: `Template pentru ${getEventTypeLabel(eventForm.value.eventType)}`,
        fields: fields,
        isActive: true,
        createdAt: new Date().toISOString(),
        updatedAt: new Date().toISOString()
      }
      // Inițializăm datele formularului
      if (eventFormTemplate.value?.fields) {
        eventFormTemplate.value.fields.forEach((field) => {
          templateFormData.value[field.fieldName] = ''
        })
      }
    } else {
      console.log('Nu există template pentru acest tip de eveniment')
    }
  } catch (error) {
    console.error('Eroare la încărcarea template-ului:', error)
  } finally {
    isLoadingTemplate.value = false
  }
}

const validateBasicForm = () => {
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

const validateTemplateForm = () => {
  if (!eventFormTemplate.value?.fields) return true
  
  templateErrors.value = {}
  let isValid = true

  eventFormTemplate.value.fields.forEach((field) => {
    if (field.isRequired && (!templateFormData.value[field.fieldName] || 
        (typeof templateFormData.value[field.fieldName] === 'string' && 
         !templateFormData.value[field.fieldName].trim()))) {
      templateErrors.value[field.fieldName] = `${field.fieldLabel} este obligatoriu`
      isValid = false
    }
  })

  return isValid
}

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
  isSaving.value = true
  try {
    const eventData: CreateEventRequest = {
      ...eventForm.value,
      userId: Number(userStore.user?.id) || 0,
      formData: templateFormData.value
    }
    
    const response = await fetch('/api/Event', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${userStore.token}`
      },
      body: JSON.stringify(eventData)
    })

    if (response.ok) {
      showWizardModal.value = false
      resetWizard()
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
  eventToDelete.value = event || null
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

const editEvent = (event: Event) => {
  // For now, use the old modal approach for editing
  eventForm.value = { 
    ...event,
    date: new Date(event.date),
    userId: Number(userStore.user?.id) || 0
  }
  showWizardModal.value = true
}

const viewEventDetails = (event: Event) => {
  router.push(`/events/${event.id}`)
}

const manageEvent = (event: Event) => {
  router.push(`/events/${event.id}/manage`)
}

const clearFilters = () => {
  searchQuery.value = ''
  selectedEventType.value = ''
  selectedDate.value = null
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

const formatDateTime = (date: string | Date) => {
  return new Date(date).toLocaleString('ro-RO', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
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

const getFieldLabel = (fieldName: string) => {
  if (!eventFormTemplate.value?.fields) return fieldName
  const field = eventFormTemplate.value.fields.find((f) => f.fieldName === fieldName)
  return field ? field.fieldLabel : fieldName
}

const formatFieldValue = (value: any) => {
  if (value === null || value === undefined || value === '') {
    return 'Nu specificat'
  }
  if (typeof value === 'boolean') {
    return value ? 'Da' : 'Nu'
  }
  if (value instanceof Date) {
    return formatDateTime(value)
  }
  return String(value)
}

const parseOptions = (optionsString: string | null) => {
  if (!optionsString) return []
  try {
    const optionsArray = JSON.parse(optionsString)
    return optionsArray.map((option: string) => ({
      label: option,
      value: option
    }))
  } catch (error) {
    console.error('Eroare la parsarea opțiunilor:', error)
    return []
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

.help-text {
  color: #7f8c8d;
  font-size: 0.85rem;
  font-style: italic;
  margin-top: 0.25rem;
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

/* Wizard Styles */
.wizard-modal {
  max-height: 90vh;
  overflow-y: auto;
}

.wizard-progress {
  margin-bottom: 2rem;
  padding: 1rem;
  background: #f8f9fa;
  border-radius: 12px;
}

.progress-steps {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1rem;
}

.progress-step {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 0.5rem;
  flex: 1;
  position: relative;
}

.progress-step:not(:last-child)::after {
  content: '';
  position: absolute;
  top: 20px;
  left: 50%;
  width: 100%;
  height: 2px;
  background: #e9ecef;
  z-index: 1;
}

.progress-step.completed:not(:last-child)::after {
  background: #27ae60;
}

.step-number {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  background: #e9ecef;
  color: #6c757d;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: 600;
  font-size: 1.1rem;
  position: relative;
  z-index: 2;
  transition: all 0.3s ease;
}

.progress-step.active .step-number {
  background: linear-gradient(135deg, #c44569, #ff6b9d);
  color: white;
  transform: scale(1.1);
}

.progress-step.completed .step-number {
  background: #27ae60;
  color: white;
}

.step-label {
  font-size: 0.9rem;
  font-weight: 600;
  color: #6c757d;
  text-align: center;
  transition: all 0.3s ease;
}

.progress-step.active .step-label {
  color: #c44569;
}

.progress-step.completed .step-label {
  color: #27ae60;
}

.progress-bar {
  height: 4px;
  background: #e9ecef;
  border-radius: 2px;
  overflow: hidden;
}

.progress-fill {
  height: 100%;
  background: linear-gradient(135deg, #c44569, #ff6b9d);
  transition: width 0.3s ease;
}

.wizard-step {
  padding: 1rem 0;
}

.wizard-step h3 {
  font-size: 1.5rem;
  font-weight: 700;
  color: #2c3e50;
  margin-bottom: 0.5rem;
}

.step-description {
  color: #7f8c8d;
  margin-bottom: 2rem;
  font-size: 1rem;
}

.wizard-form {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.template-loading {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 3rem;
  color: #7f8c8d;
}

.template-loading p {
  margin-top: 1rem;
  font-size: 1.1rem;
}

.template-form {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.checkbox-group {
  display: flex;
  align-items: center;
  gap: 0.8rem;
}

.checkbox-label {
  font-weight: 500;
  color: #2c3e50;
  cursor: pointer;
}

.no-template {
  text-align: center;
  padding: 2rem;
  color: #7f8c8d;
  background: #f8f9fa;
  border-radius: 12px;
}

.no-template i {
  font-size: 2rem;
  color: #c44569;
  margin-bottom: 1rem;
}

.review-section {
  background: #f8f9fa;
  padding: 1.5rem;
  border-radius: 12px;
  margin-bottom: 1rem;
}

.review-section h4 {
  font-size: 1.2rem;
  font-weight: 700;
  color: #2c3e50;
  margin-bottom: 1rem;
  border-bottom: 2px solid #e9ecef;
  padding-bottom: 0.5rem;
}

.review-item {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  padding: 0.8rem 0;
  border-bottom: 1px solid #e9ecef;
}

.review-item:last-child {
  border-bottom: none;
}

.review-item strong {
  color: #2c3e50;
  font-weight: 600;
  min-width: 120px;
}

.template-review {
  margin-top: 1.5rem;
  padding-top: 1.5rem;
  border-top: 2px solid #e9ecef;
}

.wizard-footer {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 1rem;
  flex-wrap: wrap;
}

.wizard-footer .p-button {
  min-width: 120px;
}

/* Responsive Wizard */
@media (max-width: 768px) {
  .wizard-modal {
    width: 95vw !important;
    max-width: 95vw !important;
  }
  
  .progress-steps {
    flex-direction: column;
    gap: 1rem;
  }
  
  .progress-step:not(:last-child)::after {
    display: none;
  }
  
  .step-label {
    font-size: 0.8rem;
  }
  
  .wizard-footer {
    flex-direction: column;
    align-items: stretch;
  }
  
  .wizard-footer .p-button {
    width: 100%;
  }
  
  .review-item {
    flex-direction: column;
    gap: 0.5rem;
  }
  
  .review-item strong {
    min-width: auto;
  }
}
</style>
