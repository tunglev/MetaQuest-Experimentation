using System;

public class Data 
{
	private DateTime m_startTime;
	private DateTime m_endTime;
	private bool m_isVisible ;
	private bool m_hasAudio;
	private SphericalCoord m_sphericalPos;
	private float m_errorAngle;
    public Data()//
	{
		m_startTime = DateTime.Now;
    }
	public Data HasAudio(bool val)
	{
		m_hasAudio= val;
		return this;
	}
	public Data IsVisible(bool val)
	{
		m_isVisible = val;
		return this;
	}
	public Data SetPos(SphericalCoord pos)
	{
		m_sphericalPos = pos;
		return this;
	}
	public Data SetErrorAngle(float val)
	{
		m_errorAngle = val;
		return this;
	}
	public static string COLUMNS = "Start time, End time, Duration(s), isVisible, hasAudio, Position, Error Angle (deg)";
	override public string ToString()
	{
        m_endTime = DateTime.Now;
        return $"{m_startTime:s}, {m_endTime:s}, {m_endTime.Subtract(m_startTime).TotalSeconds}, {m_isVisible},  {m_hasAudio}, {m_sphericalPos}, {m_errorAngle}";
	}
}
