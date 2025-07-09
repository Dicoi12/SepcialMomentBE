export interface Event {
  id: number;
  title: string;
  eventType: string;
  date: Date | string;
  location: string;
  description: string;
  userId: number;
  formData?: Record<string, any>;
  weddingGuests?: any[];
  tables?: any[];
  weddingExpenses?: any[];
}

export interface EventFormField {
  id: number;
  eventType: string;
  fieldName: string;
  fieldType: 'text' | 'textarea' | 'number' | 'select' | 'checkbox' | 'date' | 'time';
  description: string;
  isRequired: boolean;
  createdAt: string;
  updatedAt: string | null;
  fieldLabel: string;
  displayOrder: number;
  options: string | null;
  validationRules: string | null;
  helpText: string;
  eventForms: any[];
}

export interface EventFormTemplate {
  id: number;
  eventType: string;
  name: string;
  description: string;
  fields: EventFormField[];
  isActive: boolean;
  createdAt: Date | string;
  updatedAt: Date | string;
}

export interface CreateEventRequest {
  title: string;
  eventType: string;
  date: Date;
  location: string;
  description: string;
  userId: number;
  formData?: Record<string, any>;
}

export interface UpdateEventRequest extends CreateEventRequest {
  id: number;
}

export type EventType = 'nunta' | 'Baptism' | 'Anniversary' | 'Birthday' | 'Other'; 