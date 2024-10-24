using System;
using UnityEngine;

public class Data 
{
	private DateTime m_startTime;
	private DateTime m_endTime;
	private bool m_isVisible ;
	private SphericalCoord m_sphericalPos;
	private float m_errorAngle;
    public Data()//
	{
		m_startTime = DateTime.UtcNow;
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
	public Data SetAngle(float val)
	{
		m_errorAngle = val;
		return this;
	}
	override public string ToString()
	{
        m_endTime = DateTime.UtcNow;
        return $"[startTime: {m_startTime:g}, endTime: {m_endTime:g}, duration: {m_endTime.Subtract(m_startTime).TotalSeconds}s, visible: {m_isVisible}, pos: {m_sphericalPos}, error: {m_errorAngle} degree]";
	}
}
