//连线方法
//MPU-UNO
//VCC-VCC
//GND-GND
//SCL-A5
//SDA-A4
//INT-2 (Optional)

#include "Wire.h"
#include "I2Cdev.h"
#include "MPU6050.h"
#include "math.h"

#define Angular_sen  250 //默认角速度灵敏度，最大对应250°/s
#define Acceler_sen   2 //默认加速度灵敏度，2g

MPU6050 accelgyro; 
                  
int16_t ax, ay, az, gx, gy, gz;             //加速度计陀螺仪原始数据  
float aax=0, aay=0,aaz=0;                   //加速度变量
float agx=0, agy=0, agz=0;                  //角速度变量  
long axo = 0, ayo = 0, azo = 0;             //加速度计偏移量  
long gxo = 0, gyo = 0, gzo = 0;             //陀螺仪偏移量  
  
float pi = 3.1415926535;  
float fRad2Deg = 57.295779513f; //将弧度转为角度的乘数
float AcceRatio = 32767.0/Acceler_sen;      //加速度计比例系数  
float GyroRatio = 32767.0/Angular_sen;     //陀螺仪比例系数  
  
void setup()  
{  
    Wire.begin();  
    Serial.begin(9600);  
    accelgyro.initialize(); //初始化  

    unsigned short times = 1000;             //采样次数  
    for(int i=0;i<times;i++)  
    {  
        accelgyro.getMotion6(&ax, &ay, &az, &gx, &gy, &gz); //读取六轴原始数值  
        axo += ax; ayo += ay; azo += (az-AcceRatio);      //采样和  
        gxo += gx; gyo += gy; gzo += gz;  
    }  
    axo /= times; ayo /= times; azo /= times; //计算加速度计偏移  
    gxo /= times; gyo /= times; gzo /= times; //计算陀螺仪偏移 
}  

void loop()  
{  
    accelgyro.getMotion6(&ax, &ay, &az, &gx, &gy, &gz); //读取六轴原始数值  
  
    float aax = (ax-axo) / AcceRatio;  //x轴加速度  
    float aay = (ay-ayo) / AcceRatio;  //y轴加速度  
    float aaz = (az-azo) / AcceRatio;  //z轴加速度  
    float agx = (gx-gxo) / GyroRatio; //x轴角速度  
    float agy = (gy-gyo) / GyroRatio; //y轴角速度  
    float agz = (gz-gzo) / GyroRatio; //z轴角速度 
    float norm = sqrt(pow(aax,2)+pow(aay,2)+pow(aaz,2));
    int pitch = acos(sqrt(pow(aay,2)+pow(aaz,2))/norm)*180.0/pi; //pitch角
    int roll = acos(sqrt(pow(aax,2)+pow(aaz,2))/norm)*180.0/pi; //roll倾角

     if(aay>0 && -roll<0) 
    { 
      Serial.println("left");
     }
     else if(aay<0 && roll>0)
     {
       Serial.println("right");
     }
     else if(roll == 0)
     {
      Serial.println("stop");
     }
    delay(10);  
}

