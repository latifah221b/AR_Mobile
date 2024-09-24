import Foundation
import CoreMotion


@objc public class SwiftToUnity: NSObject
{
    @objc public static let shared = SwiftToUnity()
    private let pedometer = CMPedometer()


    @objc public func UnityOnStart()
    {
        if CMPedometer.isStepCountingAvailable() {
            pedometer.startUpdates(from: Date()) { data, error in
                guard let data = data, error == nil else { return }
                let steps = "\(data.numberOfSteps)"
                UnitySendMessage("StepCounter", "OnMessageReceived", steps)
            }
        }
    }
}
