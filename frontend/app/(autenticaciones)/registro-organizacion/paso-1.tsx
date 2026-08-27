import React, { useState } from 'react';
import {
  StyleSheet,
  Text,
  View,
  TouchableOpacity,
  ScrollView,
  TextInput,
  KeyboardAvoidingView,
  Platform,
  Image,
  KeyboardTypeOptions,
} from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { useRouter } from 'expo-router';
import {
  ArrowLeft,
  User,
  Phone,
  Upload,
  ArrowRight,
  Lock,
} from 'lucide-react-native';

// --- DEFINICIÓN DE TIPOS (TYPESCRIPT) ---
// El signo "?" significa que la propiedad es opcional
interface FormInputProps {
  label?: string;
  placeholder?: string;
  icon?: any;
  multiline?: boolean;
  showCount?: boolean;
  keyboardType?: KeyboardTypeOptions;
}

interface UploadBoxProps {
  label: string;
  subLabel: string;
}

// --- COMPONENTES REUTILIZABLES ---

const FormInput = ({
  label,
  placeholder,
  icon: Icon,
  multiline = false,
  showCount = false,
  keyboardType = 'default',
}: FormInputProps) => {
  const [value, setValue] = useState('');
  const maxChars = 150;

  return (
    <View style={styles.inputGroup}>
      {label && <Text style={styles.label}>{label}</Text>}
      <View
        style={[
          styles.inputContainer,
          multiline && styles.inputContainerMultiline,
        ]}
      >
        {Icon && (
          <View style={styles.iconWrapper}>
            <Icon color="#45C264" size={20} />
          </View>
        )}
        <TextInput
          style={[styles.input, multiline && styles.inputMultiline]}
          placeholder={placeholder}
          placeholderTextColor="#A0AEC0"
          value={value}
          onChangeText={(text) => {
            if (showCount && text.length > maxChars) return;
            setValue(text);
          }}
          multiline={multiline}
          keyboardType={keyboardType}
        />
      </View>
      {showCount && (
        <Text style={styles.charCount}>{`${value.length}/${maxChars}`}</Text>
      )}
    </View>
  );
};

// 2. Caja para subir archivos
const UploadBox = ({ label, subLabel }: UploadBoxProps) => (
  <View style={styles.inputGroup}>
    <Text style={styles.label}>{label}</Text>
    <TouchableOpacity style={styles.uploadContainer}>
      <Upload color="#45C264" size={20} style={styles.uploadIcon} />
      <View>
        <Text style={styles.uploadTitle}>
          Subir {label.includes('Logo') ? 'logo' : 'Documento'}
        </Text>
        <Text style={styles.uploadSubtitle}>{subLabel}</Text>
      </View>
    </TouchableOpacity>
  </View>
);

// --- PANTALLA PRINCIPAL ---

export default function RegisterOrgStep1Screen() {
  const router = useRouter();

  return (
    <SafeAreaView style={styles.safeArea}>
      <KeyboardAvoidingView
        behavior={Platform.OS === 'ios' ? 'padding' : 'height'}
        style={styles.keyboardView}
      >
        {/* Encabezado fijo */}
        <View style={styles.header}>
          <TouchableOpacity
            onPress={() => router.back()}
            style={styles.backButton}
          >
            <ArrowLeft color="#1A2E2B" size={24} />
          </TouchableOpacity>
        </View>

        <ScrollView
          showsVerticalScrollIndicator={false}
          contentContainerStyle={styles.scrollContent}
        >
          {/* Sección de Introducción */}
          <View style={styles.introSection}>
            <View style={styles.logoCircle}>
              <Image
                source={require('../../../assets/images/logo.png')}
                style={styles.logoImage}
                resizeMode="contain"
              />
            </View>
            <Text style={styles.mainTitle}>Acceso institucional</Text>
            <Text style={styles.mainSubtitle}>
              Información y apoyo para la{'\n'}comunidad productora
            </Text>
          </View>

          {/* Barra de Progreso */}
          <View style={styles.progressSection}>
            <Text style={styles.sectionTitle}>
              Información de la Institución
            </Text>
            <Text style={styles.sectionSubtitle}>
              Completa los datos de su organización
            </Text>

            <View style={styles.progressBar}>
              {/* Paso 1 Activo */}
              <View style={[styles.stepCircle, styles.stepActive]}>
                <Text style={styles.stepTextActive}>1</Text>
              </View>
              {/* Línea conectora */}
              <View style={styles.stepLine} />
              {/* Paso 2 Inactivo */}
              <View style={styles.stepCircle}>
                <Text style={styles.stepText}>2</Text>
              </View>
            </View>
          </View>

          {/* Formulario */}
          <View style={styles.formSection}>
            <FormInput
              label="Nombre Institución"
              placeholder="Ej. Cooperativa El Madroño"
            />
            <FormInput
              label="Tipo de Organización"
              placeholder="Seleccione una opción"
            />

            <Text style={styles.label}>Nombre del Responsable</Text>
            <View style={{ marginBottom: 16 }}>
              <FormInput placeholder="Nombre completo" icon={User} />
              <View style={{ height: 12 }} />
              <FormInput
                placeholder="Ej. 8888 1234"
                icon={Phone}
                keyboardType="phone-pad"
              />
            </View>

            {/* Fila de Departamento y Municipio */}
            <View style={styles.row}>
              <View style={styles.halfWidth}>
                <FormInput label="Departamento" placeholder="Seleccione" />
              </View>
              <View style={styles.halfWidth}>
                <FormInput label="Municipio" placeholder="Seleccione" />
              </View>
            </View>

            <FormInput
              label="Dirección"
              placeholder="Dirección o ubicación principal"
            />

            <FormInput
              label="Descripción breve"
              placeholder="Describe brevemente la labor de tu organización"
              multiline={true}
              showCount={true}
            />

            <UploadBox
              label="Logo de la institución"
              subLabel="PNG o JPG (máx. 2MB)"
            />
            <UploadBox
              label="Documento de verificación"
              subLabel="PDF (máx. 5MB)"
            />

            {/* Botón Siguiente */}
            <TouchableOpacity
              style={styles.submitButton}
              onPress={() => router.push('/registro-organizacion/paso-2')}
            >
              <Text style={styles.submitButtonText}>Siguiente</Text>
              <ArrowRight color="#FFFFFF" size={20} />
            </TouchableOpacity>

            {/* Footer de seguridad */}
            <View style={styles.securityFooter}>
              <Text style={styles.securityText}>
                Tu información está protegida
              </Text>
              <Lock color="#ED8936" size={14} style={{ marginLeft: 6 }} />
            </View>
          </View>
        </ScrollView>
      </KeyboardAvoidingView>
    </SafeAreaView>
  );
}

// --- ESTILOS ---

const styles = StyleSheet.create({
  safeArea: { flex: 1, backgroundColor: '#FFFFFF' },
  keyboardView: { flex: 1 },
  header: {
    paddingHorizontal: 24,
    paddingTop: 12,
    paddingBottom: 8,
  },
  backButton: { width: 40, height: 40, justifyContent: 'center' },
  scrollContent: { paddingBottom: 40 },

  // Introducción
  introSection: { alignItems: 'center', marginTop: 10, paddingHorizontal: 24 },
  logoCircle: {
    width: 80,
    height: 80,
    backgroundColor: '#F0FDF4',
    borderRadius: 40,
    justifyContent: 'center',
    alignItems: 'center',
    marginBottom: 16,
  },
  logoImage: { width: 45, height: 45 },
  mainTitle: {
    fontSize: 22,
    fontWeight: '700',
    color: '#2F855A',
    marginBottom: 6,
  },
  mainSubtitle: {
    fontSize: 14,
    color: '#718096',
    textAlign: 'center',
    lineHeight: 20,
  },

  // Progreso
  progressSection: {
    alignItems: 'center',
    marginTop: 30,
    paddingHorizontal: 24,
  },
  sectionTitle: {
    fontSize: 15,
    fontWeight: '700',
    color: '#1A2E2B',
    marginBottom: 4,
  },
  sectionSubtitle: { fontSize: 13, color: '#718096', marginBottom: 20 },
  progressBar: {
    flexDirection: 'row',
    alignItems: 'center',
    width: '100%',
    paddingHorizontal: 40,
  },
  stepCircle: {
    width: 28,
    height: 28,
    borderRadius: 14,
    backgroundColor: '#FFFFFF',
    borderWidth: 2,
    borderColor: '#CBD5E0',
    justifyContent: 'center',
    alignItems: 'center',
  },
  stepActive: { backgroundColor: '#45C264', borderColor: '#45C264' },
  stepText: { fontSize: 13, fontWeight: '700', color: '#A0AEC0' },
  stepTextActive: { fontSize: 13, fontWeight: '700', color: '#FFFFFF' },
  stepLine: {
    flex: 1,
    height: 2,
    backgroundColor: '#CBD5E0',
    marginHorizontal: 8,
  },

  // Formulario
  formSection: { marginTop: 30, paddingHorizontal: 24 },
  inputGroup: { marginBottom: 18 },
  label: { fontSize: 13, fontWeight: '600', color: '#1A2E2B', marginBottom: 8 },
  inputContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    borderWidth: 1,
    borderColor: '#CBD5E0',
    borderRadius: 10,
    minHeight: 48,
    paddingHorizontal: 14,
  },
  inputContainerMultiline: {
    alignItems: 'flex-start',
    paddingTop: 12,
    minHeight: 100,
  },
  iconWrapper: { marginRight: 10 },
  input: { flex: 1, fontSize: 14, color: '#1A2E2B' },
  inputMultiline: { textAlignVertical: 'top' },
  charCount: {
    fontSize: 11,
    color: '#A0AEC0',
    textAlign: 'right',
    marginTop: 4,
  },

  row: { flexDirection: 'row', justifyContent: 'space-between' },
  halfWidth: { width: '47%' },

  // Upload
  uploadContainer: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
    borderWidth: 1.5,
    borderColor: '#45C264',
    borderStyle: 'dashed',
    borderRadius: 10,
    paddingVertical: 16,
  },
  uploadIcon: { marginRight: 10 },
  uploadTitle: { fontSize: 13, fontWeight: '600', color: '#45C264' },
  uploadSubtitle: { fontSize: 11, color: '#A0AEC0', textAlign: 'center' },

  // Botón
  submitButton: {
    backgroundColor: '#45C264',
    borderRadius: 25,
    flexDirection: 'row',
    height: 52,
    justifyContent: 'center',
    alignItems: 'center',
    marginTop: 20,
    marginBottom: 16,
  },
  submitButtonText: {
    fontSize: 16,
    fontWeight: '600',
    color: '#FFFFFF',
    marginRight: 8,
  },

  // Footer
  securityFooter: {
    flexDirection: 'row',
    justifyContent: 'center',
    alignItems: 'center',
  },
  securityText: { fontSize: 12, color: '#A0AEC0' },
});
